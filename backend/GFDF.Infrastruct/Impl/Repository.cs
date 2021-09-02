using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Dapper.Extensions;
using System.Text;
using GFDF.Infrastruct.Core.Data;
using GFDF.Infrastruct.Core;
using System.Reflection;
using System.Collections.Concurrent;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace GFDF.Infrastruct.Impl
{
    /// <summary>
    /// Repository基类
    /// </summary>
    public class Repository
    {
        private static readonly ConcurrentDictionary<Type, List<PropertyInfo>> _paramCache = new ConcurrentDictionary<Type, List<PropertyInfo>>();
        public Repository() { }
        public Repository(string connCfg) { this.connCfg = connCfg; }

        private string connCfg;
        /// <summary>
        /// 获取表名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public string GetTableName<T>() where T : class
        {
            var tAttribute = (TableAttribute)typeof(T).GetCustomAttributes(typeof(TableAttribute), true)[0];
            return tAttribute.Name;
            //var tableName = SqlMapperExtensions.GetTableName(typeof(T));
        }

        /// <summary>
        /// 根据主键ID获取记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="primaryId"></param>
        /// <returns></returns>
        public T GetById<T>(long primaryId, string tableName = "") where T : class
        {
            using (IDbConnection conn = SessionFactory.CreateConnection(connCfg))
            {
                var tblname = string.IsNullOrEmpty(tableName) ? GetTableName<T>() : tableName;
                return conn.QueryFirstOrDefault<T>($"select * from {tblname} where id={primaryId}");
            }
        }

        public IEnumerable<T> QueryList<T>(object condition, string table, string columns = "*", bool isOr = false, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            var tblName = string.IsNullOrWhiteSpace(table) ? GetTableName<T>() : table;
            using (IDbConnection conn = SessionFactory.CreateConnection(connCfg))
            {
                return conn.Query<T>(BuildQuerySQL(condition, tblName, columns, isOr), condition, transaction, true, commandTimeout);
            }
        }
        private static string BuildQuerySQL(dynamic condition, string table, string selectPart = "*", bool isOr = false)
        {
            if (condition is string)
                return string.Format("SELECT {2} FROM {0} WHERE {1}", table, condition, selectPart);

            var conditionObj = condition as object;
            //if(conditionObj is IEnumerable<KeyValuePair<string,object>>) {
            //}
            List<string> properties = GetProperties(conditionObj);
            if (properties.Count == 0)
            {
                return string.Format("SELECT {1} FROM {0}", table, selectPart);
            }
            var separator = isOr ? " OR " : " AND ";
            var wherePart = string.Join(separator, properties.Select(p => p + " = @" + p));
            return string.Format("SELECT {2} FROM {0} WHERE {1}", table, wherePart, selectPart);
        }
        private static List<string> GetProperties(object obj)
        {
            if (obj == null)
            {
                return new List<string>();
            }
            if (obj is DynamicParameters)
            {
                return (obj as DynamicParameters).ParameterNames.ToList();
            }
            if (obj is IEnumerable<KeyValuePair<string, object>>)
            {
                return (obj as IEnumerable<KeyValuePair<string, object>>).Select(x => x.Key).ToList();
            }
            return GetPropertyInfos(obj).Select(x => x.Name).ToList();
        }
        private static List<PropertyInfo> GetPropertyInfos(object obj)
        {
            if (obj == null)
            {
                return new List<PropertyInfo>();
            }
            List<PropertyInfo> properties;
            if (_paramCache.TryGetValue(obj.GetType(), out properties)) return properties.ToList();
            properties = obj.GetType().GetProperties(BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public).ToList();
            _paramCache[obj.GetType()] = properties;
            return properties;
        }

        //public IEnumerable<T> GetDynWhere<T>(object objs) where T : class
        //{
        //    AssertHelper.EnsureNotNull(objs);
        //    var tblName = GetTableName<T>();
        //    var where = string.Join(" and ", (objs as object).GetType().GetProperties().Select(obj => string.Format("[{0}] = @{1}", obj.Name, obj.Name)));
        //    string sql = $"select * from {tblName} where {where}";

        //    using (IDbConnection conn = SessionFactory.CreateConnection(connCfg))
        //    {
        //        return conn.Query<T>(sql, objs);
        //    }
        //}

        /// <summary>
        /// 根据多个Id获取多个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ids"></param>
        /// <returns></returns>
        public IEnumerable<T> GetByIds<T>(IList<dynamic> ids) where T : class
        {
            var tblName = GetTableName<T>();
            var idsin = string.Join(",", ids.ToArray<dynamic>());
            var sql = string.Format("SELECT * FROM {0} WHERE Id in (@ids)", tblName);

            using (IDbConnection conn = SessionFactory.CreateConnection(connCfg))
            {
                return conn.Query<T>(sql, new { ids = idsin });
            }
        }

        /// <summary>
        /// 获取全部数据集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> GetAll<T>() where T : BaseEntity
        {
            using (IDbConnection conn = SessionFactory.CreateConnection(connCfg))
            {
                return conn.GetAll<T>().Where(obj => !obj.isdel).OrderByDescending(m => m.id);
            }
        }
        /// <summary>
        /// 获取单个值
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public T QuerySingle<T>(string sql, object param = null)
        {
            using (var conn = SessionFactory.CreateConnection(connCfg))
            {
                return conn.QuerySingleOrDefault<T>(sql, param);
            }
        }


        /// <summary>
        /// 根据条件筛选出数据集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="buffered"></param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(string sql, dynamic param = null, bool buffered = true) where T : class
        {
            using (IDbConnection conn = SessionFactory.CreateConnection(connCfg))
            {
                if (sql.Contains("@tableName@")) sql = sql.Replace("@tableName@", GetTableName<T>());
                return conn.Query<T>(sql, param as object, null, buffered);
            }
        }

        /// <summary>
        /// 根据表达式筛选
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="buffered"></param>
        /// <param name="splitOn"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(IDbConnection conn, string sql, Func<TFirst, TSecond, TReturn> map,
            dynamic param = null, IDbTransaction transaction = null, bool buffered = true,
            string splitOn = "Id", int? commandTimeout = null)
        {
            return SqlMapper.Query(conn, sql, map, param as object, transaction, buffered, splitOn);
        }

        /// <summary>
        /// 查询列表数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="sort"></param>
        /// <param name="buffered"></param>
        /// <returns></returns>
        //public IEnumerable<T> GetList<T>(IDbConnection conn, T predicate = null, IList<SqlMapperExtensions.ISort> sort = null,
        //    bool buffered = false) where T : class
        //{
        //    return conn.GetList<T>(predicate, sort, null, null, buffered);
        //}

        /// <summary>
        /// 分页方法调用示例：
        /// 1. 单一条件
        //  using (SqlConnection cn = new SqlConnection(_connectionString))
        //  {
        //    cn.Open();
        //
        //    //排序字段
        //    var sortList = new List<DapperExtensions.ISort>();
        //    sortList.Add(new DapperExtensions.Sort { PropertyName = "ID", Ascending = false });
        //
        //    var predicate = Predicates.Field<Person>(f => f.Active, Operator.Eq, true);
        //    List<Person> list = cn.GetPaged<Person>(cn, query.PageIndex, query.PageSize, 
        //            predicate, sortList, false).ToList();
        //
        //    cn.Close();
        //  }
        //
        //  2. 组合条件
        //  using (SqlConnection cn = new SqlConnection(_connectionString))
        //  {
        //    cn.Open();
        //
        //    //排序字段
        //    var sortList = new List<DapperExtensions.ISort>();
        //    sortList.Add(new DapperExtensions.Sort { PropertyName = "ID", Ascending = false });
        //
        //    var pg = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
        //    pg.Predicates.Add(Predicates.Field<Person>(f => f.Active, Operator.Eq, true));
        //    pg.Predicates.Add(Predicates.Field<Person>(f => f.LastName, Operator.Like, "Br%"));
        //
        //    List<Person> list = cn.GetPaged<Person>(cn, query.PageIndex, query.PageSize, 
        //            pg, sortList, false).ToList();
        //
        //    cn.Close();
        //  }
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="allRowsCount">总记录数</param>
        /// <param name="predicate">条件</param>
        /// <param name="sort">排序</param>
        /// <param name="buffered">缓存</param>
        /// <returns></returns>
        //public IEnumerable<T> GetPaged<T>(IDbConnection conn, int pageIndex, int pageSize, object predicate,
        //    IList<ISort> sort = null, bool buffered = false) where T : class
        //{
        //    return null;
        //}

        /// <summary>
        /// 分页查询（存储过程）
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="conn">连接</param>
        /// <param name="pager">分页对象</param>
        /// <param name="buffered">缓存</param>
        /// <returns></returns>
        //public IEnumerable<T> GetPaged<T>(IDbConnection conn, Pager pager, bool buffered = false) where T : class
        //{
        //    var tblName = string.IsNullOrEmpty(pager.TableName) ? GetTableName<T>() : pager.TableName;
        //    var keyFieldName = string.IsNullOrEmpty(pager.KeyFieldName) ? "ID" : pager.KeyFieldName;

        //    var p = new DynamicParameters();
        //    p.Add("@pageIndex", pager.PageIndex);
        //    p.Add("@pageSize", pager.PageSize);
        //    p.Add("@tblName", tblName);
        //    p.Add("@fldName", keyFieldName);
        //    p.Add("@isDesc", pager.IsDesc);
        //    p.Add("@strWhere", pager.StrWhere);
        //    p.Add("@fldOrder", pager.FieldOrder);

        //    return conn.Query<T>("pr_sys_QueryPaged", p, null, buffered, null, CommandType.StoredProcedure);
        //}
        public virtual IEnumerable<T> GetPaged<T>(int pageSize, int pageIndex, string strWhere, string filedOrder, out int recordCount, string tableName = "") where T : class
        {
            using (var conn = SessionFactory.CreateConnection(connCfg))
            {
                StringBuilder strSql = new StringBuilder();
                if (pageSize == 0) strSql.Append($"select * from (");
                strSql.Append($"select * from {(string.IsNullOrEmpty(tableName) ? GetTableName<T>() : tableName)}");
                if (strWhere.Trim() != "")
                {
                    strSql.Append(" where" + strWhere);
                }
                if (pageSize == 0) strSql.Append($") as a");
                recordCount = Convert.ToInt32(conn.ExecuteScalar(PagingHelper.CreateCountingSql(strSql.ToString())));
                if (pageSize == 0) strSql.Append($" ORDER BY {filedOrder}");
                return pageSize != 0 ? conn.Query<T>(PagingHelper.CreatePagingSqlunOrder(recordCount, pageSize, pageIndex, strSql.ToString(), filedOrder)).ToList() : conn.Query<T>(strSql.ToString()).ToList();
            }
        }

        public virtual IEnumerable<T> GetPagedEx<T>(int pageSize, int pageIndex, string strSql, string filedOrder, out int recordCount) where T : class
        {
            using (var conn = SessionFactory.CreateConnection(connCfg))
            {
                recordCount = Convert.ToInt32(conn.ExecuteScalar(PagingHelper.CreateCountingSql(strSql.ToString())));
                if (pageSize == 0) strSql = $"select * from ({strSql}) as a ORDER BY {filedOrder}";
                return pageSize != 0 ? conn.Query<T>(PagingHelper.CreatePagingSqlunOrder(recordCount, pageSize, pageIndex, strSql.ToString(), filedOrder)).ToList() : conn.Query<T>(strSql.ToString()).ToList();
            }
        }

        ///// <summary>
        ///// 统计记录总数
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="predicate">条件</param>
        ///// <param name="buffered">缓存</param>
        ///// <returns></returns>
        //public int Count<T>(IDbConnection conn, T predicate, bool buffered = false) where T : class
        //{
        //    return conn.Count<T>(predicate);
        //}


        /// <summary>
        /// 获取多实体集合
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public SqlMapper.GridReader GetMultiple(string sql, out IDbConnection conn, dynamic param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            conn = SessionFactory.CreateConnection(connCfg);
            return SqlMapper.QueryMultiple(conn, sql, param);
        }

        /// <summary>
        /// 执行sql操作
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int Execute(string sql, dynamic param = null)
        {
            using (IDbConnection conn = SessionFactory.CreateConnection(connCfg))
            {
                return conn.Execute(sql, param as object);
            }
        }

        /// <summary>
        /// 执行sql操作
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int Execute(IDbConnection conn, string sql, dynamic param = null, IDbTransaction transaction = null)
        {
            return conn.Execute(sql, param as object, transaction);
        }

        /// <summary>
        /// 执行sql操作(事务)
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int ExecuteTransaction(string sql, dynamic param = null)
        {
            var session = SessionFactory.CreateSession();
            session.BeginTrans();
            try
            {
                var val = session.Connection.Execute(sql, param as object, session.Transaction);
                session.Commit();
                return val;
            }
            catch (System.Exception)
            {
                session.Rollback();
                throw;
            }
            finally
            {
                session.Dispose();
            }
        }

        /// <summary>
        /// 执行command操作
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public int ExecuteCommand(IDbCommand cmd)
        {
            return cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int ExecuteProc(string procName, DynamicParameters param = null)
        {
            using (IDbConnection conn = SessionFactory.CreateConnection(connCfg))
            {
                return conn.Execute(procName, param, null, null, CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="procName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int ExecuteProc(IDbConnection conn, string procName, DynamicParameters param = null)
        {
            return conn.Execute(procName, param, null, null, CommandType.StoredProcedure);
        }

        /// <summary>
        /// 存储过程执行方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public IList<T> ExecProcQuery<T>(string procName, DynamicParameters param)
            where T : class
        {
            using (IDbConnection conn = SessionFactory.CreateConnection(connCfg))
            {
                IList<T> list = conn.Query<T>(procName, param, null, false, null, CommandType.StoredProcedure).ToList<T>();
                return list;
            }
        }

        /// <summary>
        /// 存储过程执行方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="procName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public IList<T> ExecProcQuery<T>(IDbConnection conn, string procName, DynamicParameters param)
            where T : class
        {
            IList<T> list = conn.Query<T>(procName, param, null, false, null, CommandType.StoredProcedure).ToList<T>();
            return list;
        }

        /// <summary>
        /// 执行SQL语句，返回查询结果
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="sql"></param>
        /// <param name="buffered"></param>
        /// <returns></returns>
        public object ExecuteScalar(IDbConnection conn, string sql, bool buffered = false)
        {
            var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;

            return ExecuteScalar(conn, cmd);
        }

        /// <summary>
        /// 执行SQL语句，并返回数值
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="conn"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public object ExecuteScalar(IDbConnection conn, IDbCommand cmd)
        {
            try
            {
                bool wasClosed = conn.State == ConnectionState.Closed;
                if (wasClosed) conn.Open();

                return cmd.ExecuteScalar();
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }

        /// <summary>
        /// 插入实体
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="entity">实体</param>
        /// <returns>主键自增值</returns>
        public void Insert<T>(T entity) where T : class
        {
            using (IDbConnection conn = SessionFactory.CreateConnection(connCfg))
                conn.Insert<T>(entity);
        }

        /// <summary>
        /// 插入单条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public dynamic Insert<T>(IDbConnection conn, T entity, IDbTransaction transaction = null) where T : class
        {
            dynamic result = conn.Insert<T>(entity, transaction);
            return result;
        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体</param>
        /// <returns>是否成功</returns>
        public bool Update<T>(T entity) where T : class
        {
            var session = SessionFactory.CreateSession();

            session.BeginTrans();
            try
            {
                var isOk = Update<T>(session.Connection, entity, session.Transaction);
                session.Commit();

                return isOk;
            }
            catch (System.Exception)
            {
                session.Rollback();
                throw;
            }
            finally
            {
                session.Dispose();
            }
        }

        /// <summary>
        /// 更新单条记录
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体</param>
        /// <returns>是否成功</returns>
        public bool Update<T>(IDbConnection conn, T entity, IDbTransaction transaction = null) where T : class
        {
            bool isOk = conn.Update<T>(entity, transaction);
            return isOk;
        }


        /// <summary>
        /// 批量更新（）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityList"></param>
        /// <returns></returns>
        public bool UpdateBatch<T>(IEnumerable<T> entityList) where T : class
        {
            bool isOk = false;
            var session = SessionFactory.CreateSession();
            session.BeginTrans();
            try
            {
                foreach (var item in entityList)
                {
                    Update<T>(session.Connection, item, session.Transaction);
                }
                session.Commit();
            }
            catch (System.Exception)
            {
                session.Rollback();
                throw;
            }
            finally
            {
                session.Dispose();
            }
            isOk = true;
            return isOk;
        }

        public int UpdateDynamic<T>(dynamic entity) where T : class
        {
            // var objs = entity;
            AssertHelper.EnsureNotNull(entity);
            var name = GetTableName<T>();
            AssertHelper.EnsureNotNull(name);

            //DynamicParameters dp = new DynamicParameters();
            //var s1 = SqlMapperExtensions.TypePropertiesCache(typeof(T));
            //var jsonSerializer = new JsonSerializer();

            //foreach (JProperty item in entity)
            //{
            //    var p = s1.Where(obj => obj.Name == item.Name).FirstOrDefault();
            //    if (p != null) {
            //        dp.Add(item.Name, item.Value.ToObject(p.PropertyType, jsonSerializer));
            //    }
            //}

            var ts = SqlMapperExtensions.TypePropertiesCache(typeof(T)).Select(obj => obj.Name);
            IEnumerable<JProperty> t = (entity as JObject).Properties().ToArray();
            var js = t.Select(obj => obj.Name);
            AssertHelper.EnsureTrue(js.Contains("id"), "id必须存在!");
            var sb = new StringBuilder();
            sb.AppendFormat("update {0} set ", name);
            sb.Append(string.Join(",", ts.Intersect(js).Where(obj => obj != "id").Select(obj => string.Format("[{0}] = @{1}", obj, obj))));
            //sb.Append(string.Join(",", GetProperties(dp).Where(obj => obj != "id").Select(obj => string.Format("[{0}] = @{1}", obj, obj))));
            sb.Append(" where id=@id");
            return Execute(sb.ToString(), (entity as JObject).ToObject<T>());
        }



        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete<T>(dynamic id) where T : class
        {
            var session = SessionFactory.CreateSession();

            session.BeginTrans();
            try
            {
                var isOk = Delete<T>(session.Connection, id, session.Transaction);
                session.Commit();
                return isOk;
            }
            catch (System.Exception)
            {
                session.Rollback();
                throw;
            }
            finally
            {
                session.Dispose();
            }
        }

        /// <summary>
        /// 删除单条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="primaryId"></param>
        /// <returns></returns>
        private bool Delete<T>(IDbConnection conn, dynamic primaryId, IDbTransaction transaction = null) where T : class
        {
            var entity = GetById<T>(primaryId);
            var obj = entity as T;
            bool isOk = conn.Delete<T>(obj, transaction);
            return isOk;
        }

        /// <summary>
        /// 批量插入功能
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityList"></param>
        public void InsertBatch<T>(IEnumerable<T> entityList) where T : class
        {
            using (IDbConnection conn = SessionFactory.CreateConnection(connCfg))
            {
                conn.Insert<IEnumerable<T>>(entityList);
            }
        }

        public int DeleteBatch<T>(IEnumerable<dynamic> ids) where T : class
        {
            var session = SessionFactory.CreateSession();

            session.BeginTrans();
            try
            {
                var isOk = DeleteBatch<T>(session.Connection, ids, session.Transaction);
                session.Commit();
                return isOk;
            }
            catch (System.Exception)
            {
                session.Rollback();
                throw;
            }
            finally
            {
                session.Dispose();
            }
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ids"></param>
        /// <returns></returns>
        private int DeleteBatch<T>(IDbConnection conn, IEnumerable<dynamic> ids, IDbTransaction transaction = null) where T : class
        {
            var tblName = GetTableName<T>();
            var idsin = string.Join(",", ids.ToArray<dynamic>());
            var sql = string.Format("DELETE FROM dbo.{0} WHERE ID in ({1})", tblName, idsin);
            var result = SqlMapper.Execute(conn, sql, null, transaction);

            return result;
        }

        public SqlMapper.GridReader GetMultiple(string sql, IDbConnection conn, dynamic param = null,
     IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return SqlMapper.QueryMultiple(conn, sql, param, transaction, commandTimeout, commandType);
        }



        /// <summary>  
        /// 双TOP二分法生成分页SQL类(支持MSSQL、ACCESS)
        /// </summary>  
        public static class PagingHelper
        {
            /// <summary>
            /// 获取分页SQL语句，排序字段需要构成唯一记录
            /// </summary>
            /// <param name="_recordCount">记录总数</param>
            /// <param name="_pageSize">每页记录数</param>
            /// <param name="_pageIndex">当前页数</param>
            /// <param name="_safeSql">SQL查询语句</param>
            /// <param name="_orderField">排序字段，多个则用“,”隔开</param>
            /// <returns>分页SQL语句</returns>
            public static string CreatePagingSql(int _recordCount, int _pageSize, int _pageIndex, string _safeSql, string _orderField)
            {
                //重新组合排序字段，防止有错误
                string[] arrStrOrders = _orderField.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                StringBuilder sbOriginalOrder = new StringBuilder(); //原排序字段
                StringBuilder sbReverseOrder = new StringBuilder(); //与原排序字段相反，用于分页
                for (int i = 0; i < arrStrOrders.Length; i++)
                {
                    arrStrOrders[i] = arrStrOrders[i].Trim();  //去除前后空格
                    if (i != 0)
                    {
                        sbOriginalOrder.Append(", ");
                        sbReverseOrder.Append(", ");
                    }
                    sbOriginalOrder.Append(arrStrOrders[i]);

                    int index = arrStrOrders[i].IndexOf(" "); //判断是否有升降标识
                    if (index > 0)
                    {
                        //替换升降标识，分页所需
                        bool flag = arrStrOrders[i].IndexOf(" DESC", StringComparison.OrdinalIgnoreCase) != -1;
                        sbReverseOrder.AppendFormat("{0} {1}", arrStrOrders[i].Remove(index), flag ? "ASC" : "DESC");
                    }
                    else
                    {
                        sbReverseOrder.AppendFormat("{0} DESC", arrStrOrders[i]);
                    }
                }

                //计算总页数
                _pageSize = _pageSize == 0 ? _recordCount : _pageSize;
                int pageCount = (_recordCount + _pageSize - 1) / _pageSize;

                //检查当前页数
                if (_pageIndex < 1)
                {
                    _pageIndex = 1;
                }
                else if (_pageIndex > pageCount)
                {
                    _pageIndex = pageCount;
                }

                StringBuilder sbSql = new StringBuilder();
                //第一页时，直接使用TOP n，而不进行分页查询
                if (_pageIndex == 1)
                {
                    sbSql.AppendFormat(" SELECT TOP {0} * ", _pageSize);
                    sbSql.AppendFormat(" FROM ({0}) AS T ", _safeSql);
                    sbSql.AppendFormat(" ORDER BY {0} ", sbOriginalOrder.ToString());
                }
                //最后一页时，减少一个TOP
                else if (_pageIndex == pageCount)
                {
                    sbSql.Append(" SELECT * FROM ");
                    sbSql.Append(" ( ");
                    sbSql.AppendFormat(" SELECT TOP {0} * ", _recordCount - _pageSize * (_pageIndex - 1));
                    sbSql.AppendFormat(" FROM ({0}) AS T ", _safeSql);
                    sbSql.AppendFormat(" ORDER BY {0} ", sbReverseOrder.ToString());
                    sbSql.Append(" ) AS T ");
                    sbSql.AppendFormat(" ORDER BY {0} ", sbOriginalOrder.ToString());
                }
                //前半页数时的分页
                else if (_pageIndex <= (pageCount / 2 + pageCount % 2) + 1)
                {
                    sbSql.Append(" SELECT * FROM ");
                    sbSql.Append(" ( ");
                    sbSql.AppendFormat(" SELECT TOP {0} * FROM ", _pageSize);
                    sbSql.Append(" ( ");
                    sbSql.AppendFormat(" SELECT TOP {0} * ", _pageSize * _pageIndex);
                    sbSql.AppendFormat(" FROM ({0}) AS T ", _safeSql);
                    sbSql.AppendFormat(" ORDER BY {0} ", sbOriginalOrder.ToString());
                    sbSql.Append(" ) AS T ");
                    sbSql.AppendFormat(" ORDER BY {0} ", sbReverseOrder.ToString());
                    sbSql.Append(" ) AS T ");
                    sbSql.AppendFormat(" ORDER BY {0} ", sbOriginalOrder.ToString());
                }
                //后半页数时的分页
                else
                {
                    sbSql.AppendFormat(" SELECT TOP {0} * FROM ", _pageSize);
                    sbSql.Append(" ( ");
                    sbSql.AppendFormat(" SELECT TOP {0} * ", ((_recordCount % _pageSize) + _pageSize * (pageCount - _pageIndex) + 1));
                    sbSql.AppendFormat(" FROM ({0}) AS T ", _safeSql);
                    sbSql.AppendFormat(" ORDER BY {0} ", sbReverseOrder.ToString());
                    sbSql.Append(" ) AS T ");
                    sbSql.AppendFormat(" ORDER BY {0} ", sbOriginalOrder.ToString());
                }
                return sbSql.ToString();
            }

            /// <summary>
            /// 获取分页SQL语句，排序字段需要构成唯一记录
            /// </summary>
            /// <param name="_recordCount">记录总数</param>
            /// <param name="_pageSize">每页记录数</param>
            /// <param name="_pageIndex">当前页数</param>
            /// <param name="_safeSql">SQL查询语句</param>
            /// <param name="_orderField">排序字段，多个则用“,”隔开</param>
            /// <returns>分页SQL语句</returns>
            public static string CreatePagingSqlunOrder(int _recordCount, int _pageSize, int _pageIndex, string _safeSql)
            {

                //计算总页数
                _pageSize = _pageSize == 0 ? _recordCount : _pageSize;
                int pageCount = (_recordCount + _pageSize - 1) / _pageSize;

                //检查当前页数
                if (_pageIndex < 1)
                {
                    _pageIndex = 1;
                }
                else if (_pageIndex > pageCount)
                {
                    _pageIndex = pageCount;
                }
                //拼接SQL字符串，加上ROW_NUMBER函数进行分页
                StringBuilder newSafeSql = new StringBuilder();
                newSafeSql.AppendFormat("SELECT ROW_NUMBER() OVER(ORDER BY ID) as row_number,");
                newSafeSql.Append(_safeSql.Substring(_safeSql.ToUpper().IndexOf("SELECT") + 6));

                //拼接成最终的SQL语句
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append("SELECT * FROM (");
                sbSql.Append(newSafeSql.ToString());
                sbSql.Append(") AS T");
                sbSql.AppendFormat(" WHERE row_number between {0} and {1}", ((_pageIndex - 1) * _pageSize) + 1, _pageIndex * _pageSize);

                return sbSql.ToString();
            }

            /// <summary>
            /// 获取分页SQL语句，排序字段需要构成唯一记录
            /// </summary>
            /// <param name="_recordCount">记录总数</param>
            /// <param name="_pageSize">每页记录数</param>
            /// <param name="_pageIndex">当前页数</param>
            /// <param name="_safeSql">SQL查询语句</param>
            /// <param name="_orderField">排序字段，多个则用“,”隔开</param>
            /// <returns>分页SQL语句</returns>
            public static string CreatePagingSqlunOrder(int _recordCount, int _pageSize, int _pageIndex, string _safeSql, string _orderField)
            {
                //重新组合排序字段，防止有错误
                string[] arrStrOrders = _orderField.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                StringBuilder sbOriginalOrder = new StringBuilder(); //原排序字段
                if (arrStrOrders.Length == 0) sbOriginalOrder.Append("id");
                for (int i = 0; i < arrStrOrders.Length; i++)
                {
                    arrStrOrders[i] = arrStrOrders[i].Trim();  //去除前后空格
                    if (i != 0) sbOriginalOrder.Append(", ");
                    sbOriginalOrder.Append(arrStrOrders[i]);
                }

                //计算总页数
                _pageSize = _pageSize == 0 ? _recordCount : _pageSize;

                //检查当前页数
                if (_pageIndex < 1) _pageIndex = 1;

                //拼接成最终的SQL语句
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append($"SELECT * FROM ({_safeSql}) AS T");
                sbSql.AppendFormat(" ORDER BY {0} offset {1} rows fetch next {2} rows only;", sbOriginalOrder.ToString(), (_pageIndex - 1) * _pageSize, _pageSize);

                return sbSql.ToString();
            }

            /// <summary>
            /// 获取记录总数SQL语句
            /// </summary>
            /// <param name="_n">限定记录数</param>
            /// <param name="_safeSql">SQL查询语句</param>
            /// <returns>记录总数SQL语句</returns>
            public static string CreateTopnSql(int _n, string _safeSql)
            {
                return string.Format(" SELECT TOP {0} * FROM ({1}) AS T ", _n, _safeSql);
            }

            /// <summary>
            /// 获取记录总数SQL语句
            /// </summary>
            /// <param name="_safeSql">SQL查询语句</param>
            /// <returns>记录总数SQL语句</returns>
            public static string CreateCountingSql(string _safeSql)
            {
                return string.Format(" SELECT COUNT(1) AS RecordCount FROM ({0}) AS T ", _safeSql);
            }
        }
    }


}
