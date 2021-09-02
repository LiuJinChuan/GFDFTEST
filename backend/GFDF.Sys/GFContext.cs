using System;
using System.Collections.Generic;
using System.Linq;
using GFDF.Infrastruct.Core;
using GFDF.Infrastruct.Storage;
using GFDF.Infrastruct.Impl;
using GFDP.Sys.Entity;
using GFDF.Infrastruct.Extensions;
using FluentScheduler;
using GFDP.Sys.Job;
using System.Dynamic;
using GFDP.Sys.Service;
using GFDF.Infrastruct.Configuration;

namespace GFDP.Sys
{
    public class GFContext
    {
        public static ILogger logger;
        public static IdWorker idworker;
        public static Repository repository;
        public static CacheService cache;
        public static JWTHelper jwt;
        public static IStorageProvider storage;
        public static dynamic cfg = new ExpandoObject();
        public static void load()
        {
            EventBus.Subscribe(Do);
            EventBus.Subscribe(DoNotify);   //通知

            JobManager.Initialize(new ApiJobs());
            JobManager.JobException += (exinfo) => EventBus.Emit("Job Exception", exinfo);
            //cfg = CfgManage.Instance.GetSection<dynamic>("cfg_sys", BaseService.GetCfgByForm);
        }

        public static void Do(EventBus.EventData eventdata)
        {
            dynamic obj = eventdata.BussData;
            switch (eventdata.EventSource)
            {
                case var a when a.Contains("RoleEntity.Create"):
                    PermisEntity entity = new PermisEntity()
                    {
                        id = idworker.nextId(),
                        role = eventdata.BussData.id
                    };
                    repository.Insert<PermisEntity>(entity);
                    break;


                //配置页面操作，页面按钮，按钮接口参数{"btns":[{"cname":"","callcode":"","webapi":0}]}
                case var a when a.Contains("PageEntity.Create"):
                    //dynamic extobj = eventdata.BussData.extobj;
                    //List<ButtonEntity> buttons = new List<ButtonEntity>();
                    //foreach (var btnItem in extobj.btns)
                    //{
                    //    buttons.Add(new ButtonEntity() { id = GFContext.idworker.nextId(), callcode = (string)btnItem.callcode, cname = (string)btnItem.cname, page = (long)eventdata.BussData.id, webapi = (long)btnItem.webapi });
                    //}
                    //GFContext.repository.InsertBatch(buttons);
                    break;

                case var a when a.Contains("RoleEntity.Delete"):
                    repository.Execute("delete from sys_permission where role=@id", eventdata.BussData);
                    break;

                case var a when a.Contains("FormValueEntity.Update"):
                    //配置发生改变，转为配置消息提醒
                    EventBus.Emit($"cfg_{obj.id}_change", obj);
                    break;

                case var a when a.Contains("token.Pass"):
                    if (obj.aud == "USER" && !GFContext.cache.Contain($"U_{obj.sub}"))
                    {
                        var model = GFContext.repository.Query<dynamic>($"select sys_user.id,sys_user.cname,dept,account,sys_dept.cname as deptname,sys_user.roles from sys_user left join sys_dept on dept=sys_dept.id where sys_user.id={obj.sub}").FirstOrDefault();
                        AssertHelper.EnsureNotNull(model, $"未找到id为{obj.sub}的用户！");
                        GFContext.cache.Insert($"U_{model.id}", new { model.id, model.account, model.cname, model.dept, model.deptname, model.roles, obj.token }, new TimeSpan(0, 0, 0, 7200));
                    }
                    break;

                case var a when a.Contains("Login_Out"):
                    string token = obj?.token ?? "";
                    if (!string.IsNullOrWhiteSpace(token)) LoseToken(token);
                    break;

                case var a when a.Contains("RoleEntity.Delete"):
                    repository.Execute("delete from sys_permission where role=@id", eventdata.BussData);
                    break;

                case "Common.Upload":
                    //文件上传的记录 (入库）
                    FileResourceEntity ent = new FileResourceEntity
                    {
                        id = GFContext.idworker.nextId(),
                        orgname = obj.orgname,
                        newname = obj.newname,
                        url = obj.url,
                        creator = (long)obj.creator,
                        filetype = System.IO.Path.GetExtension((string)obj.orgname),
                        length = obj.length,
                        contenttype = obj.contenttype,
                        container = obj.container,
                        contentmd5 = obj.contentmd5,
                        etag = obj.etag,
                        lastmodified = obj.lastmodified
                    };

                    //TODO: 使用SHA256记录文件水印，以便去重、 防止修改等，  可以异步方式处理

                    repository.Insert<FileResourceEntity>(ent);
                    break;
                case "common.audit":
                    var info = new AuditLogEntity
                    {
                        id = GFContext.idworker.nextId(),
                        userid = obj.userid,
                        controller = obj.controller,
                        action = obj.action,
                        parameters = (obj.parameters as Object).ToString2(),
                        exception = (obj.exception as Object).ToString2(),
                        result = (obj.result as Object).ToString2(),
                        useragent = obj.useragent,
                        ip = obj.ip,
                        host = obj.host,
                        duration = obj.duration,
                    };
                    GFContext.repository.Insert<AuditLogEntity>(info);
                    break;
                case "QrtzTaskEntity.UpRunState":  //调度任务的启停

                    break;
            }
        }

        public static void DoNotify(EventBus.EventData eventdata)
        {
        }

        public static bool ValidToken(string token, out dynamic data, out string msg)
        {
            data = new System.Dynamic.ExpandoObject();
            msg = "";
            Dictionary<string, object> items = new Dictionary<string, object>();
            if (GFContext.cache.Contain($"T_BL_{token}"))
            {
                msg = "token已失效!";
                GFContext.logger.Info($"ValidToken:token({token})已失效!");
                return false;
            }
            try
            {
                items = GFContext.jwt.Decode(token);
            }
            catch (Exception ex)
            {
                msg = "token已过期!";
                GFContext.logger.Info($"ValidToken:token({token})已过期或签名验证失败!");
                GFContext.logger.Error(ex);
                EventBus.Emit("token.expire", items);
                return false;
            }
            if ("6b93fbda7c78c904" != items["iss"].ToString())  //TODO：iss应存放于u-key 或 加密文件中。
            {
                msg = "token来源不正确!";
                GFContext.logger.Info($"ValidToken:token({token})来源不正确!");
                return false;
            }
            //JWT Decode已经处理TokenExpired 和 SignatureVerification
            //if ((long)items["exp"] < DateTimeOffset.UtcNow.AddSeconds(10).ToUnixTimeSeconds())
            //{
            //    msg = "token已过期!";
            //    GFContext.logger.Info($"ValidToken:token({token})已过期!");
            //    EventBus.Emit("token.expire", items);
            //    return false;
            //}
            if ((long)items["nbf"] > DateTimeOffset.UtcNow.ToUnixTimeSeconds())
            {
                msg = "token未到启用时间!";
                GFContext.logger.Info($"ValidToken:token({token})未到启用时间!");
                return false;
            }
            if (GFContext.cache.Contain($"T_BL_{items["sub"]}"))
            {
                msg = "用户已属黑名单!";
                GFContext.logger.Info($"ValidToken:token({token}-{items["sub"]})用户已属黑名单!");
                return false;
            }
            if (GFContext.cache.Contain($"T_BL_{items["jti"]}"))
            {
                msg = "一次性token已使用过!";
                GFContext.logger.Info($"ValidToken:token({token}-{items["jti"]})属一次性token,并已使用过!");
                return false;
            }
            if ((long)items["jti"] != 0) GFContext.cache.Insert($"T_BL_{items["jti"]}", true, DateTimeOffset.FromUnixTimeSeconds((long)items["exp"]).DateTime);

            //data.sub = items["sub"];
            //data.aud = items["aud"];
            //data.data = items["data"];
            //data.token = token;
            data = new { sub = items["sub"], aud = items["aud"], data = items["data"], token };
            return true;
        }
        public static string IssueToken(object data, string aud = "USER", string sub = "", double timeout = 7200, long jti = 0)
        {
            return jwt.Encode(data, aud, sub, timeout, jti);
        }
        public static bool LoseToken(string token)
        {
            Dictionary<string, object> items = GFContext.jwt.Decode(token);
            GFContext.cache.Insert($"T_BL_{token}", true, DateTimeOffset.FromUnixTimeSeconds((long)items["exp"]).LocalDateTime);
            return true;
        }
    }
}
