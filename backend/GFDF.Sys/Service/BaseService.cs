using Dapper.Extensions;
using GFDF.Infrastruct.Core;
using GFDF.Infrastruct.Extensions;
using GFDP.Sys.Entity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GFDP.Sys.Service
{
    public class BaseService
    {
        public static int Update<T>(JObject param) where T : BaseEntity
        {
            var name = GFContext.repository.GetTableName<T>();
            AssertHelper.EnsureNotNull(name);

            var ts = SqlMapperExtensions.TypePropertiesCache(typeof(T)).Select(obj => obj.Name);
            IEnumerable<JProperty> t = param.Properties().Where(obj => !string.IsNullOrEmpty(obj.Value.ToString()) && ts.Contains(obj.Name)).ToArray(); //去除值为null的字段 不属于对象表里的字段
            var js = t.Select(obj => obj.Name);
            AssertHelper.EnsureTrue(js.Contains("id"), "id必须存在!");

            var sb = new StringBuilder();
            sb.AppendFormat("update {0} set ", name);
            sb.Append(string.Join(",", js.Where(obj => obj != "id").Select(obj => string.Format("[{0}] = @{1}", obj, obj))));
            sb.Append(" where id in (@id) and cstatus&4=0");

            T model = new JObject(t).ToObject<T>();
            return GFContext.repository.Execute(sb.ToString(), model);
        }

        public static dynamic GetCfgByForm(string cfgName)
        {
            var model = GFContext.repository.QueryList<FormValueEntity>(new { callcode = cfgName }, "").FirstOrDefault();
            AssertHelper.EnsureNotNull(model, $"未找到{cfgName}对应的配置信息");
            return model.extobj;
        }

        public static int Delete<T>(long id) where T : BaseEntity
        {
            var tAttribute = (TableAttribute)typeof(T).GetCustomAttributes(typeof(TableAttribute), true)[0];
            string sql = tAttribute.softdel ? $"update {tAttribute.Name} set cstatus=cstatus|2 where id =@id and cstatus&4=0" : $"delete {tAttribute.Name} where id =@id and cstatus&4=0";
            return GFContext.repository.Execute(sql, new { id });
        }

        public static int DeleteBatch<T>(IEnumerable<dynamic> ids) where T : BaseEntity
        {
            var tAttribute = (TableAttribute)typeof(T).GetCustomAttributes(typeof(TableAttribute), true)[0];
            string sql = tAttribute.softdel?$"update {tAttribute.Name} set cstatus=cstatus|2 where id in @ids and cstatus&4=0":$"delete {tAttribute.Name} where id in @ids and cstatus&4=0";
            return GFContext.repository.Execute(sql, new { ids });
        }

        public void Insert<T>(T entity) where T : BaseEntity
        {
            entity.id = GFContext.idworker.nextId();
            GFContext.repository.Insert<T>(entity);
        }
        public void InsertBatch<T>(IList<T> entities) where T : BaseEntity
        {
            entities.ForEach(obj => obj.id = GFContext.idworker.nextId());
            GFContext.repository.InsertBatch<T>(entities);
        }
    }
}
