using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using FluentScheduler;
using GFDF.Infrastruct.Core;
using GFDF.Infrastruct.Impl;
using GFDP.Sys.Entity;
using GFDP.Sys.Filter;

namespace GFDP.Sys.Controllers
{
    [RoutePrefix("qrtz/task")]
    public class QrtzTaskController : BaseController<Qrtz_TaskEntity>
    {

        [HttpPost]
        [Route("uprunstate")]
        public virtual ResponseResult UpState(dynamic param)
        {
            string sql = $"update qrtz_task set flag=(flag & (4095 - 12) | (@state*4)) where id = @id";
            GFContext.repository.Execute(sql, new { id = (long)param.id, state = (int)param.runstate });
            EventBus.Emit($"QrtzTaskEntity.UpRunState", new { param.id, param.runstate });
            return ResponseResult.Success();
        }
    };
    [RoutePrefix("sys/message")]
    public class MessageController : BaseController<MessageEntity> { };

    [RoutePrefix("sys/message_user")]
    public class MessageUserController : BaseController<Message_UserEntity>
    {
        public MessageUserController()
        {
            this.isview = true;
        }
    };

    [RoutePrefix("sys/page")]
    public class PageController : BaseController<PageEntity> { };

    [RoutePrefix("sys/button")]
    public class ButtonController : BaseController<ButtonEntity> { };

    [RoutePrefix("sys/webapi")]
    public class WebapiController : BaseController<WebapiEntity> { };

    [RoutePrefix("sys/form")]
    public class FormController : BaseController<FormEntity> { }

    [RoutePrefix("sys/region")]
    public class RegionController : BaseController<Data_RegionEntity>
    {
        [AllowAnonymous]
        public override ResponseResult List(string json = "")
        {
            return base.List(json);
        }

        [AllowAnonymous]
        public override dynamic View(long id)
        {
            return base.View(id);
        }
    }

    [RoutePrefix("sys/formvalue")]
    public class FormValueController : BaseController<FormValueEntity>
    {
        /// <summary>
        /// 获取ext数据
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("ext/{callcode}")]
        [AllowAnonymous]
        public ResponseResult ViewExt(string callcode)
        {
            var tblName = GFContext.repository.GetTableName<FormValueEntity>();
            var model = GFContext.repository.QueryList<FormValueEntity>(new { callcode }, tblName).FirstOrDefault();
            AssertHelper.EnsureNotNull(model);
            ResponseResult result = ResponseResult.Success();
            result.Message = model.ext;
            result.data = model.extobj;
            return result;
        }
    }

    [RoutePrefix("sys/role")]
    public class RoleController : BaseController<RoleEntity>
    {
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPut, Route("permis")]
        [EventBus(bAudit = true, bWrap = true)]
        public void UpdatePermis(PermisEntity param)
        {
            AssertHelper.EnsureNotNull(param);
            string sql = "update sys_permission set menu=@menu,page=@page,button=@button,webapi=@webapi where role=@id";
            GFContext.repository.Execute(sql, param);
            EventBus.Emit($"PermisEntity.UpdatePermis", param);
        }
    }

    [RoutePrefix("sys/permis")]
    public class PermisController : BaseController<PermisEntity> { }

    [RoutePrefix("sys/notice")]
    public class NoticeController : BaseController<NoticeEntity>
    {

        [AllowAnonymous]
        public override ResponseResult List(string json = "")
        {
            return base.List(json);
        }

        [AllowAnonymous]
        public override dynamic View(long id)
        {
            return base.View(id);
        }
    }

    [RoutePrefix("sys/menu")]
    public class MenuController : BaseController<MenuEntity> { }

    [RoutePrefix("sys/dict")]
    public class DictController : BaseController<DictEntity>
    {
        /// <summary>
        /// 获取子级字典
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpGet, Route("callcode/{callcode}")]
        [AllowAnonymous]
        public ResponseResult CallCode(string callcode)
        {
            var model = GFContext.repository.Query<DictEntity>($"select * from @tableName@ where callcode='{callcode}'").FirstOrDefault();
            if (model == null) return ResponseResult.Error("无效的调用标识");
            string sql = $"select cname,cvalue from sys_dict where pid={model.id}";
            var list = GFContext.repository.Query<dynamic>(sql);
            return ResponseResult.Success(list);
        }
    }

    [RoutePrefix("sys/dept")]
    public class DeptController : BaseController<DeptEntity>
    {
        /// <summary>
        /// 各父部门
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("parents/{id:long}")]
        [EventBus(bWrap = true)]
        public IEnumerable<DeptEntity> GetDepts(long deptid)
        {
            string sql = "WITH TEMP AS (SELECT * FROM sys_dept WHERE Id=@id UNION ALL SELECT T0.* FROM TEMP,sys_dept T0 WHERE TEMP.pid=T0.Id) SELECT * FROM TEMP;";
            return GFContext.repository.Query<DeptEntity>(sql, new { id = deptid });
        }
    }

    [RoutePrefix("base/audit")]
    public class AuditLogController : BaseController<AuditLogEntity>
    {
        [HttpGet, Route("loginlog")]
        [EventBus(bWrap = true)]
        public IEnumerable<dynamic> LoginLog() => GFContext.repository.QueryList<AuditLogEntity>(new { controller = "Common", action = "Login" }, "base_auditlog").Select(obj => new { obj.id, obj.ip, obj.host, obj.duration, obj.action, obj.controller, obj.parameters, obj.useragent, obj.userid });
    }

    [RoutePrefix("base/fileres")]
    public class FileResourceController : BaseController<FileResourceEntity> { }

    [RoutePrefix("cache")]
    public class CacheController : ApiController
    {
        [HttpGet, Route("{prefix}")]
        [EventBus(bWrap = true)]
        public Dictionary<string, object> ListByPrefix(string prefix) => GFContext.cache.Get<dynamic>((key) => key.StartsWith(prefix));

        [HttpDelete, Route("{prefix}")]
        [EventBus(bWrap = true)]
        public void Delete(string prefix) => GFContext.cache.RemoveAll((key) => key.StartsWith(prefix));
    }

    [RoutePrefix("schedule")]
    public class ScheduleController : ApiController
    {
        [HttpGet, Route("all")]
        [EventBus(bWrap = true)]
        public IEnumerable<Schedule> ListAll() => JobManager.AllSchedules;


        [HttpDelete, Route("{jobname}")]
        [EventBus(bWrap = true)]
        public void Delete(string jobname) => JobManager.RemoveJob(jobname);

        /// <summary>
        /// 获取数据详情
        /// </summary>
        /// <returns></returns>
        [HttpPut, Route("disable/{jobname}")]
        [EventBus(bWrap = true)]
        public void Disable(string jobname) => JobManager.GetSchedule(jobname).Disable();

        /// <summary>
        /// 获取数据详情
        /// </summary>
        /// <returns></returns>
        [HttpPut, Route("enable/{jobname}")]
        [EventBus(bWrap = true)]
        public void Enable(string jobname) => JobManager.GetSchedule(jobname).Enable();
    }
}