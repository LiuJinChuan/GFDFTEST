using FluentScheduler;
using GFDF.Infrastruct.Core;
using GFDF.Infrastruct.Extensions;
using GFDP.Sys.Filter;
using GFDP.Sys.Service;
using GFDP.Sys.Entity;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;

namespace GFDP.Sys.Controllers
{
    [RoutePrefix("other")]
    public class SysOtherController : BaseController<BaseEntity>
    {
        [HttpGet, Route("ping")]
        [AllowAnonymous]
        [EventBus(bWrap = true)]
        public void ping()
        {
            GFContext.repository.Execute("select 1");
        }

        /// <summary>
        /// 创建websocket连接
        /// </summary>
        [HttpGet]
        [Route("GetConnect")]
        [AllowAnonymous]
        public HttpResponseMessage GetConnect()
        {
            if (HttpContext.Current.IsWebSocketRequest)
            {
                HttpContext.Current.AcceptWebSocketRequest(WSHelper.GetInstance().ProcessWSChat);
            }
            return new HttpResponseMessage(HttpStatusCode.SwitchingProtocols);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        [HttpGet]
        [Route("GiveOpinion")]
        [EventBus(bWrap = true)]
        public async Task<string> GiveOpinion(string msg, string sendTo)
        {
            //1.发送消息给客户端
            bool result = await WSHelper.GetInstance().SendMsgAsync(msg, sendTo);

            //2.接收结果，若发送失败，可能客户端还未成功连接WebSocket
            return result ? "已提交。" : "发送失败，请重试。";
        }

        /// <summary>
        /// 获取连接者
        /// </summary>
        [HttpGet]
        [Route("GetLink")]
        [EventBus(bWrap = true)]
        public dynamic GetLink()
        {
            return WSHelper.GetInstance().GetLink();
        }

        /// <summary>
        /// 获取浏览信息
        /// </summary>
        [HttpPost, Route("getbrowseinfo"), AllowAnonymous]
        [EventBus(bWrap = true)]
        public void GetLogInfo(dynamic info)
        {
            Dictionary<string, object> dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(info));
            dic.Add("p", HttpContext.Current.Request.UserHostAddress);
            dic.Add("h", HttpContext.Current.Request.UserHostName);
            dic.Add("g", HttpContext.Current.Request.UserAgent);
            dic.Add("t", DateTime.Now.GetTimeStamp());
            GFContext.logger.Warn(JsonConvert.SerializeObject(dic));
        }
    }

    [RoutePrefix("fin/flow")]
    public partial class F_flowController : BaseController<FINFlowEntity>
    {
        public F_flowController()
        {
            base.refuse = 0b111100;
            base.isview = true;
        }
    };

    [RoutePrefix("fin/account")]
    public partial class F_AccountController : BaseController<FINAccountEntity>
    {
        public F_AccountController()
        {
            base.refuse = 0b111100;
            base.isview = true;
        }
    };
}
