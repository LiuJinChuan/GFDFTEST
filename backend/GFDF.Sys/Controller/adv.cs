using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using FluentScheduler;
using GFDF.Infrastruct.Core;
using GFDF.Infrastruct.Impl;
using GFDP.Sys.Entity;
using GFDP.Sys.Filter;
using System;

namespace GFDP.Sys.Controllers
{
    [RoutePrefix("adv/advert")]
    public class AdvertController : BaseController<AdvertEntity>
    {
        /// <summary>
        /// 发布广告
        /// </summary>
        /// <returns></returns>
        [Route("pub"), HttpPost, EventBus(sAllowAud = "USER", bWrap = false, bPreDup = true)]
        public ResponseResult Pub(dynamic info)
        {
            AssertHelper.EnsureNotNull(info, "数据不能为空");
            var model = GFContext.repository.GetById<AdvertEntity>((long)info.id);
            AssertHelper.EnsureNotNull(model, "广告不存在");
            GFContext.repository.ExecuteTransaction($"delete adv_advert_release where advertid={model.id};");
            string advertstr = (string)info.adverts;
            List<Advert_ReleaseEntity> entitys = new List<Advert_ReleaseEntity>();
            foreach (var item in advertstr.Split(','))
            {
                Advert_ReleaseEntity entity = new Advert_ReleaseEntity() { id = GFContext.idworker.nextId(), userid = Convert.ToInt64(Request.Properties["U_ID"]), advertid = model.id, advertsid = Convert.ToInt64(item), run_stime = (long)info.run_stime, run_etime = (long)info.run_etime, range = (string)info.range };
                entitys.Add(entity);
            }
            GFContext.repository.InsertBatch(entitys);
            int num = GFContext.repository.ExecuteTransaction($"update adv_advert set flag=1 where id={model.id};");
            AssertHelper.EnsureTrue(num > 0, "操作失败");
            return ResponseResult.Success("操作成功");
        }

        /// <summary>
        /// 暂停广告
        /// </summary>
        /// <returns></returns>
        [Route("stop/{advid:long}"), HttpGet, EventBus(sAllowAud = "USER", bWrap = false, bPreDup = true)]
        public ResponseResult Stop(long advid)
        {
            AssertHelper.EnsureFalse(advid == 0, "数据不能为空");
            int num = GFContext.repository.ExecuteTransaction($"update adv_advert set flag=0 where id={advid};delete adv_advert_release where advertid={advid};");
            AssertHelper.EnsureTrue(num > 0, "操作失败");
            return ResponseResult.Success("操作成功");
        }
    }

    [RoutePrefix("adv/advert_space")]
    public class Advert_SpaceController : BaseController<Advert_SpaceEntity> { };

    [RoutePrefix("adv/advert_release")]
    public class Advert_ReleaseController : BaseController<Advert_ReleaseEntity>
    {
        public Advert_ReleaseController()
        {
            base.isview = true;
        }

        [AllowAnonymous]
        public override ResponseResult List(string json = "")
        {
            return base.List(json);
        }
    };
}