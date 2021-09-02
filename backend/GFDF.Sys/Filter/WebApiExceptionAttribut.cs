using GFDF.Infrastruct.Core;
using GFDP.Sys;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace GFDP.Sys.Filter
{
    public class WebApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        //重写基类的异常处理方法
        public override void OnException(HttpActionExecutedContext aec)
        {
            string source = $"{ aec.ActionContext.ActionDescriptor.ControllerDescriptor.ControllerName}.{aec.ActionContext.ActionDescriptor.ActionName}";
            EventBus.Emit("WebApi.Error", new { source, args = aec.ActionContext.ActionArguments, exception = aec.Exception.ToString() });

            GFContext.logger.Error(aec.Exception);

            if (aec.Exception is NotImplementedException)
            {
                aec.Response = new HttpResponseMessage(HttpStatusCode.NotImplemented);
            }
            else if (aec.Exception is TimeoutException)
            {
                aec.Response = new HttpResponseMessage(HttpStatusCode.RequestTimeout);
            }
            else
            {
                var oResponse = new HttpResponseMessage(HttpStatusCode.OK);
                ResponseResult result = ResponseResult.Error("");

                if (aec.Exception is BaseException)
                {
                    BaseException ex = (BaseException)aec.Exception;
                    result.Message = ex.exMsg;
                    result.data = ex.BussData;
                }
                else
                {
                    result.Message = aec.Exception.Message;
                    //result.data = actionExecutedContext.ActionContext.ActionArguments;
                }
                oResponse.Content = new StringContent(JsonConvert.SerializeObject(result));
                aec.Response = oResponse;
            }
            base.OnException(aec);
        }
    }
}