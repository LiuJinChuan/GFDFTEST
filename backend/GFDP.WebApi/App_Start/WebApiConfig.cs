using GFDP.Sys.Filter;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Cors;
using System.Web.Http.Routing;

namespace GFDP.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //CORS跨域设置
            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));

            //JSON
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();

            //Web API 路由
            config.MapHttpAttributeRoutes(new CustomDirectRouteProvider());

            //异常处理 过滤器
            config.Filters.Add(new WebApiExceptionFilterAttribute());
            config.Filters.Add(new ApiAuthAttribute());
        }


        public class CustomDirectRouteProvider : DefaultDirectRouteProvider
        {
            protected override IReadOnlyList<IDirectRouteFactory> GetActionRouteFactories(HttpActionDescriptor actionDescriptor)
            {
                return actionDescriptor.GetCustomAttributes<IDirectRouteFactory>(inherit: true);
            }
        }
    }
}
