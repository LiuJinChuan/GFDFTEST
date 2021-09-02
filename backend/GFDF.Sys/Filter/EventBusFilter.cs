using GFDF.Infrastruct.Core;
using GFDF.Infrastruct.Extensions;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace GFDP.Sys.Filter
{
    public class EventBusAttribute : ActionFilterAttribute
    {
        public bool bPreDup { get; set; } = false;  //防止重复提交
        public bool bAudit { get; set; } = false;  //审计
        public bool bWrap { get; set; } = false;  //结果包装
        public string sAllowAud { get; set; } = "";  //接口允许的token aud， 多个以;号间隔
        private const string key = "_action_duration_";
        public override void OnActionExecuted(HttpActionExecutedContext aec)
        {
            if (aec.Exception != null) return;

            string source = $"{ aec.ActionContext.ActionDescriptor.ControllerDescriptor.ControllerName}.{aec.ActionContext.ActionDescriptor.ActionName}".Replace("/", ".");
            aec.Response.TryGetContentValue(out object res);
            EventBus.Emit(source, new { args = aec.ActionContext.ActionArguments, res });

            if (bAudit && aec.Request.Properties.ContainsKey(key))
            {
                var stopWatch = aec.Request.Properties[key] as Stopwatch;
                AssertHelper.EnsureNotNull(stopWatch, "ActionDuration不能为空!");
                stopWatch.Stop();

                dynamic info = new
                {
                    userid = (aec.Request.Properties.ContainsKey("U_ID") ? aec.Request.Properties["U_ID"] : ""),
                    controller = aec.ActionContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                    action = aec.ActionContext.ActionDescriptor.ActionName,
                    parameters = aec.ActionContext.ActionArguments,
                    exception = aec.Exception,
                    useragent = HttpContext.Current.Request.UserAgent,
                    result = res,
                    method = aec.Request.Method.Method,
                    ip = HttpContext.Current.Request.UserHostAddress,
                    host = HttpContext.Current.Request.UserHostName,
                    duration = stopWatch.ElapsedMilliseconds
                };
                EventBus.Emit("common.audit", info);
            }

            if (bWrap)
            {
                ResponseResult result = ResponseResult.Success();
                // 取得由 API 返回的资料
                result.data = aec.ActionContext.Response.Content?.ReadAsAsync<object>()?.Result;
                //请求是否成功
                result.code = aec.ActionContext.Response.IsSuccessStatusCode ? 0 : -1;
                //结果转为自定义消息格式
                HttpResponseMessage httpResponseMessage = toJson(result);
                // 重新封装回传格式
                aec.Response = httpResponseMessage;
            }

            if (!string.IsNullOrEmpty(sAllowAud))
            {
                string[] auds = sAllowAud.Split(';');
                AssertHelper.EnsureTrue(auds.Contains(o => o == aec.Request.Properties["U_TYPE"].ToString()),"接口不接受所含类型的token!");
            }

            base.OnActionExecuted(aec);
        }

        public override void OnActionExecuting(HttpActionContext aec)
        {
            string info = JsonConvert.SerializeObject(aec.ActionArguments);
            //GFContext.logger.Info(info);

            if (bPreDup)
            {
                string dupkey = $"DUP_{aec.ControllerContext.ControllerDescriptor.ControllerName}.{aec.ActionDescriptor.ActionName}.{info.ToMd5()}";
                AssertHelper.EnsureFalse(GFContext.cache.Contain(dupkey), "不能频繁提交重复数据!");
                GFContext.cache.Insert(dupkey, "", TimeSpan.FromSeconds(15));
            }

            if (bAudit)
            {
                var stopWatch = new Stopwatch();
                aec.Request.Properties[key] = stopWatch;
                stopWatch.Start();
            }
            //var oResponse = new HttpResponseMessage(HttpStatusCode.OK);
            //oResponse.Content = new StringContent(JsonConvert.SerializeObject(ResponseResult.Error("abc")));
            // actionContext.Response = oResponse;
            base.OnActionExecuting(aec);
        }

        public static HttpResponseMessage toJson(Object obj)
        {
            String str = (obj is String || obj is Char) ? obj.ToString() : obj.ToString2();
            HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(str, Encoding.GetEncoding("UTF-8"), "application/json") };
            return result;
        }
    }
}
