using GFDF.Infrastruct.Core;
using GFDF.Infrastruct.Extensions;
using GFDP.Sys.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace GFDP.Sys.Filter
{
    public class ApiAuthAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext ac)
        {
            if (ac.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any())
            {
                return;
            }
            //string token = string.Empty;
            //AssertHelper.EnsureTrue((ac.Request.Headers.Authorization != null) || (ac.Request.Headers.Upgrade.Where(o => o.Name == "websocket").Any() && ac.Request.Headers.Contains("Sec-WebSocket-Protocol")), "Authorization信息不能为空!");
            //if (ac.Request.Headers.Upgrade.Where(o => o.Name == "websocket").Any() && ac.Request.Headers.Contains("Sec-WebSocket-Protocol"))
            //{
            //    token = ((string[])ac.Request.Headers.GetValues("Sec-WebSocket-Protocol"))[0];
            //}
            //else
            //{
            //    token = ac.Request.Headers.Authorization.ToString();
            //}

            //AssertHelper.EnsureNotNull(ac.Request.Headers.Authorization, "Authorization信息不能为空!");
            var token = ac.Request.Headers.Authorization;
            string msg = "Authorization信息不能为空!";
            if (token != null && GFContext.ValidToken(token.ToString(), out dynamic info, out msg))
            {
                ac.Request.Properties.Add($"U_TYPE", info.aud.ToString());
                ac.Request.Properties.Add($"U_ID", info.sub.ToString());
                EventBus.Emit("token.Pass", info);
            }
            else
            {
                var oResponse = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                ResponseResult result = ResponseResult.Return(1, msg);
                oResponse.Content = new System.Net.Http.StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(result));
                ac.Response = oResponse;
            }
        }
    }
}