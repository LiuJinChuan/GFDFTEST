using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using GFDF.Infrastruct.Core;
using GFDF.Infrastruct.Extensions;
using GFDP.Sys.Service;
using GFDP.Sys.Filter;

namespace GFDP.Sys.Controllers
{
    public class BaseController<T> : ApiController where T : BaseEntity, new()
    {
        public long refuse = 0;
        public bool isview = false;

        #region Query
        /// <summary>
        /// 列表查询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("list")]
        public virtual ResponseResult List(string json = "")
        {
            AssertHelper.EnsureTrue((refuse & 64) == 0, "不允许调用此接口!");
            if (!string.IsNullOrEmpty(json)) return Query(json);
            var list = isview ? GFContext.repository.Query<dynamic>($"select * from vw_{GFContext.repository.GetTableName<T>()} order by id desc") : GFContext.repository.GetAll<T>();
            return ResponseResult.Return(0, "", 0, list, list.Count());
        }

        /// <summary>
        /// 列表查询
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("listp")]
        public virtual ResponseResult ListP(dynamic info)
        {
            AssertHelper.EnsureTrue((refuse & 64) == 0, "不允许调用此接口!");
            if (!string.IsNullOrEmpty((string)info.json)) return Query((string)info.json);
            var list = isview ? GFContext.repository.Query<dynamic>($"select * from vw_{GFContext.repository.GetTableName<T>()} order by id desc") : GFContext.repository.GetAll<T>();
            return ResponseResult.Return(0, "", 0, list, list.Count());
        }

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public virtual ResponseResult Query(string json)
        {
            DoPageParam(json, out int pageSize, out int pageIndex, out string param, out string filedOrder);
            ResponseResult result = ResponseResult.Success();
            int count = 0;
            result.data = isview ? GFContext.repository.GetPaged<dynamic>(pageSize, pageIndex, param, filedOrder, out count, "vw_" + GFContext.repository.GetTableName<T>()) : GFContext.repository.GetPaged<T>(pageSize, pageIndex, param, filedOrder, out count);
            result.count = count;
            return result;
        }

        public void DoPageParam(string json, out int pageSize, out int pageIndex, out string strParam, out string filedOrder)
        {
            if (string.IsNullOrEmpty(json)) throw new BaseException(ExEnum.Other, "获取数据失败，参数不能为空！");
            JObject jObject = (JObject)JsonConvert.DeserializeObject(json);
            pageIndex = 0;
            pageSize = 0;
            List<string> param = new List<string>();
            List<string> paramor = new List<string>();
            List<string> lis = new List<string>();
            filedOrder = string.Empty;
            foreach (var obj in jObject)
            {
                if (obj.Value.ToString() == "" || obj.Value.ToString() == "[]") continue;
                switch (obj.Key)
                {
                    case "page":
                        pageIndex = Convert.ToInt32(obj.Value);
                        if (pageIndex == 0 && pageSize == 0) pageIndex = 0;
                        break;
                    case "limit":
                        pageSize = Convert.ToInt32(obj.Value);
                        if (pageIndex == 0 && pageSize == 0) pageSize = 0;
                        break;
                    case "order":
                        JArray oObject = (JArray)JsonConvert.DeserializeObject(obj.Value.ToString());
                        List<string> paramOrder = new List<string>();
                        foreach (var oObj in oObject)
                        {
                            JObject oo = (JObject)JsonConvert.DeserializeObject(oObj.ToString());
                            foreach (var t in oo)
                            {
                                if (t.Value.ToString() == "") continue;
                                paramOrder.Add($" {t.Key} {t.Value} ");
                            }
                        }
                        filedOrder = paramOrder.Count > 0 ? string.Join(",", paramOrder) : "id desc";
                        break;
                    case "like":
                        JArray lObject = (JArray)JsonConvert.DeserializeObject(obj.Value.ToString());
                        foreach (var lObj in lObject)
                        {
                            JObject oo = (JObject)JsonConvert.DeserializeObject(lObj.ToString());
                            foreach (var t in oo)
                            {
                                if (t.Value.ToString() == "") continue;
                                param.Add($" {t.Key} like '%{t.Value.ToString().Replace(@"\", "")}%' ");
                            }
                        }
                        break;
                    case "likestart":
                        JArray sObject = (JArray)JsonConvert.DeserializeObject(obj.Value.ToString());
                        foreach (var lObj in sObject)
                        {
                            JObject oo = (JObject)JsonConvert.DeserializeObject(lObj.ToString());
                            foreach (var t in oo)
                            {
                                if (t.Value.ToString() == "") continue;
                                param.Add($" {t.Key} like '{t.Value.ToString().Replace(@"\", "")}%' ");
                            }
                        }
                        break;
                    case "likeend":
                        JArray eObject = (JArray)JsonConvert.DeserializeObject(obj.Value.ToString());
                        foreach (var lObj in eObject)
                        {
                            JObject oo = (JObject)JsonConvert.DeserializeObject(lObj.ToString());
                            foreach (var t in oo)
                            {
                                if (t.Value.ToString() == "") continue;
                                param.Add($" {t.Key} like '%{t.Value.ToString().Replace(@"\", "")}' ");
                            }
                        }
                        break;
                    case "likes":
                        JArray lsArray = (JArray)JsonConvert.DeserializeObject(obj.Value.ToString());
                        foreach (var lsObj in lsArray)
                        {
                            JObject lsObject = (JObject)JsonConvert.DeserializeObject(lsObj.ToString());
                            string sp = "";
                            if (!lsObject.ContainsKey("data")) continue;
                            if (!lsObject.ContainsKey("andor")) continue;
                            if (lsObject.ContainsKey("sp")) sp = lsObject["sp"].ToString();
                            JArray lsdArray = (JArray)JsonConvert.DeserializeObject(lsObject["data"].ToString());
                            List<string> lsd = new List<string>();
                            foreach (var lObj in lsdArray)
                            {
                                JObject oo = (JObject)JsonConvert.DeserializeObject(lObj.ToString());
                                foreach (var t in oo)
                                {
                                    if (t.Value.ToString() == "") continue;
                                    string likes = $" '{sp}'+{t.Key}+'{sp}' like '{sp}%{t.Value.ToString().Replace(@"\", "")}%{sp}' ";
                                    lsd.Add(likes);
                                }
                            }
                            if (lsd.Count > 0) lis.Add("(" + string.Join(lsObject["andor"].ToString().Trim(), lsd) + ")");
                        }
                        break;
                    case "andor":
                        JObject aoObject = (JObject)JsonConvert.DeserializeObject(obj.Value.ToString());
                        List<string> paramandor = new List<string>();
                        foreach (var t in aoObject)
                        {
                            if (t.Key.ToString() == "" || t.Value.ToString() == "") continue;
                            var tJArray = (JArray)JsonConvert.DeserializeObject(t.Value.ToString());
                            List<string> sss = new List<string>();
                            foreach (var andObj in tJArray)
                            {
                                JObject oo = (JObject)JsonConvert.DeserializeObject(andObj.ToString());
                                foreach (var t1 in oo)
                                {
                                    if (t1.Value.ToString() == "") continue;
                                    sss.Add($" {t1.Key}='{t1.Value}' ");
                                }
                            }
                            paramandor.Add("(" + string.Join("or", sss) + ")");
                        }
                        param.Add("(" + string.Join("and", paramandor) + ")");
                        break;
                    case "range":
                        JArray rObject = (JArray)JsonConvert.DeserializeObject(obj.Value.ToString());
                        foreach (var rObj in rObject)
                        {
                            JObject oo = (JObject)JsonConvert.DeserializeObject(rObj.ToString());
                            foreach (var t in oo)
                            {
                                if (t.Value.ToString() == "") continue;
                                param.Add($" {t.Key}{t.Value} ");
                            }
                        }
                        break;
                    case "ranges":
                        JArray rObjects = (JArray)JsonConvert.DeserializeObject(obj.Value.ToString());
                        foreach (var rObj in rObjects)
                        {
                            JObject oo = (JObject)JsonConvert.DeserializeObject(rObj.ToString());
                            foreach (var t in oo)
                            {
                                if (t.Value.ToString() == "") continue;
                                param.Add($" {t.Key} between '{t.Value.ToString().Split(',')[0]}' and '{t.Value.ToString().Split(',')[1]}' ");
                            }
                        }
                        break;
                    case "or":
                        JArray orObjects = (JArray)JsonConvert.DeserializeObject(obj.Value.ToString());
                        foreach (var eObj in orObjects)
                        {
                            JObject oo = (JObject)JsonConvert.DeserializeObject(eObj.ToString());
                            foreach (var t in oo)
                            {
                                if (t.Value.ToString() == "") continue;
                                paramor.Add($" {t.Key}='{t.Value}' ");
                            }
                        }
                        break;
                    case "in":
                        JArray ins = (JArray)JsonConvert.DeserializeObject(obj.Value.ToString());
                        foreach (var iObj in ins)
                        {
                            JObject oo = (JObject)JsonConvert.DeserializeObject(iObj.ToString());
                            foreach (var t in oo)
                            {
                                if (t.Value.ToString() == "") continue;
                                param.Add($" {t.Key} in({t.Value}) ");
                            }
                        }
                        break;
                    case "notin":
                        JArray notIns = (JArray)JsonConvert.DeserializeObject(obj.Value.ToString());
                        foreach (var iObj in notIns)
                        {
                            JObject oo = (JObject)JsonConvert.DeserializeObject(iObj.ToString());
                            foreach (var t in oo)
                            {
                                if (t.Value.ToString() == "") continue;
                                param.Add($" {t.Key} not in({t.Value}) ");
                            }
                        }
                        break;
                    case "noequels":
                        JArray nObjects = (JArray)JsonConvert.DeserializeObject(obj.Value.ToString());
                        foreach (var eObj in nObjects)
                        {
                            JObject oo = (JObject)JsonConvert.DeserializeObject(eObj.ToString());
                            foreach (var t in oo)
                            {
                                if (t.Value.ToString() == "") continue;
                                paramor.Add($" {t.Key}<>'{t.Value}' ");
                            }
                        }
                        break;
                    default:
                        JArray eObjects = (JArray)JsonConvert.DeserializeObject(obj.Value.ToString());
                        foreach (var eObj in eObjects)
                        {
                            JObject oo = (JObject)JsonConvert.DeserializeObject(eObj.ToString());
                            foreach (var t in oo)
                            {
                                if (t.Value.ToString() == "") continue;
                                param.Add($" {t.Key}='{t.Value}' ");
                            }
                        }
                        break;
                }
            }
            if (paramor.Count > 0) param.Add("(" + string.Join("or", paramor) + ")");
            if (lis.Count > 0) param.Add("(" + string.Join("and", lis) + ")");
            strParam = param.Count > 0 ? string.Join("and", param) : "";
        }

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="strWhere"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual ResponseResult QueryList(string strWhere, object obj = null)
        {
            string sql = $"select * from {(isview ? "vw_" : "")}@tableName@ " + $"where {strWhere}".If(!string.IsNullOrWhiteSpace(strWhere));
            ResponseResult result = ResponseResult.Success();
            var entities = isview ? GFContext.repository.Query<dynamic>(sql, obj) : GFContext.repository.Query<T>(sql, obj);
            result.data = entities;
            result.count = entities.Count();
            return result;
        }

        /// <summary>
        /// 获取数据详情
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("view/{id:long}")]
        [EventBus(bWrap = true)]
        public virtual dynamic View(long id)
        {
            AssertHelper.EnsureTrue((refuse & 128) == 0, "不允许调用此接口!");
            return isview ? GFContext.repository.GetById<dynamic>(id, "vw_" + GFContext.repository.GetTableName<T>()) : GFContext.repository.GetById<T>(id);
        }

        /// <summary>
        /// 获取数据详情
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("view/{callcode}")]
        [EventBus(bWrap = true)]
        public virtual dynamic View(string callcode)
        {
            AssertHelper.EnsureTrue((refuse & 256) == 0, "不允许调用此接口!");
            AssertHelper.NotNullOrWhiteSpace(callcode, "获取信息失败，参数不能为空！");
            string tblname = (isview ? "vw_" : "") + GFContext.repository.GetTableName<T>();

            //var condition = new List<KeyValuePair<string, object>>();
            //condition.Add(new KeyValuePair<string, object>("callcode", callcode));

            return isview ? GFContext.repository.QueryList<dynamic>(new { callcode }, tblname).FirstOrDefault() : GFContext.repository.QueryList<T>(new { callcode }, tblname).FirstOrDefault();
        }
        #endregion

        #region command
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [EventBus(bPreDup = true, bAudit = true)]
        public virtual ResponseResult Create(T entity)
        {
            AssertHelper.EnsureTrue((refuse & 1) == 0, "不允许调用此接口!");
            entity.id = GFContext.idworker.nextId();
            GFContext.repository.Insert<T>(entity);
            EventBus.Emit($"{typeof(T)}.Create", entity);
            return ResponseResult.Success(entity.id);
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("batchadd")]
        [EventBus(bPreDup = true, bAudit = true, bWrap = true)]
        public virtual void BatchAdd(List<T> entitys)
        {
            AssertHelper.EnsureTrue((refuse & 2) == 0, "不允许调用此接口!");
            entitys.ForEach(item => item.id = GFContext.idworker.nextId());
            GFContext.repository.InsertBatch<T>(entitys);
            EventBus.Emit($"{typeof(T)}.BatchAdd", entitys);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("")]
        [EventBus(bAudit = true, bWrap = true)]
        public virtual void Update(JObject param)
        {
            AssertHelper.EnsureTrue((refuse & 4) == 0, "不允许调用此接口!");
            AssertHelper.EnsureNotNull(param);
            AssertHelper.EnsureTrue(param.ContainsKey("id"), "id必须存在!");
            int affected = BaseService.Update<T>(param);
            AssertHelper.EnsureTrue(affected > 0, "记录不存在或已锁定不允许修改。");
            EventBus.Emit($"{typeof(T)}.Update", param);
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("upstatus")]
        [EventBus(bAudit = true, bWrap = true)]
        public virtual void UpStatus(long[] ids, int cstatus)
        {
            AssertHelper.EnsureTrue((refuse & 8) == 0, "不允许调用此接口!");
            string sql = $"update @tableName@ set cstatus=@cstatus where id in @ids and cstatus&4=0";
            int affected = GFContext.repository.Execute(sql, new { ids, cstatus });
            AssertHelper.EnsureTrue(affected > 0, "记录不存在或已锁定不允许修改。");
            EventBus.Emit($"{typeof(T)}.UpStatus", new { ids, cstatus });
        }

        /// <summary>
        /// 单个删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id:long}")]
        [EventBus(bAudit = true, bWrap = true)]
        public virtual void Delete(long id)
        {
            AssertHelper.EnsureTrue((refuse & 16) == 0, "不允许调用此接口!");
            int affected = BaseService.Delete<T>(id);
            AssertHelper.EnsureTrue(affected > 0, "记录不存在或已锁定不允许删除。");
            EventBus.Emit($"{typeof(T)}.Delete", new { id });
        }

        /// <summary>
        /// 批量删除，允许部分成功
        /// </summary>
        /// <param name="ls"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("batchdel")]
        [EventBus(bAudit = true, bWrap = true)]
        public virtual string BatchDelete(List<dynamic> ls)
        {
            AssertHelper.EnsureTrue((refuse & 32) == 0, "不允许调用此接口!");
            if (ls == null || ls.Count <= 0) throw new ArgumentNullException();
            int affected = BaseService.DeleteBatch<T>(ls);
            AssertHelper.EnsureTrue(affected > 0, "记录不存在或已锁定不允许删除。");
            EventBus.Emit($"{typeof(T)}.BatchDelete", ls);
            return affected < ls.Count ? "部分记录不存在或已锁定不允许删除。" : "";
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("unlock/{id:long}")]
        [EventBus(bPreDup = true, bAudit = true, bWrap = true)]
        public virtual void unlock(long id)
        {
            AssertHelper.EnsureTrue((refuse & 512) == 0, "不允许调用此接口!");
            int affected = GFContext.repository.Execute($"update { GFContext.repository.GetTableName<T>() } set cstatus=cstatus&(65535-4) where id={id}");
            AssertHelper.EnsureTrue(affected > 0, $"{id}对应记录不存在。");
            EventBus.Emit($"{typeof(T)}.Unlock", id);
        }


        #endregion

        public bool EstimateRe(object objs)
        {
            AssertHelper.EnsureNotNull(objs);
            var name = GFContext.repository.GetTableName<T>();
            var where = string.Join(" and ", (objs as object).GetType().GetProperties().Select(obj => string.Format("[{0}] = @{1}", obj.Name, obj.Name)));
            string sql = $"select count(1) from {name} where {where}";

            long num = GFContext.repository.QuerySingle<long>(sql, objs);
            return num > 0;
        }
    }
}