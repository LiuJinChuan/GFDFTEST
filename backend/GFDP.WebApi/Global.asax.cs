using System.Web.Http;
using GFDP.WebApi;
using Newtonsoft.Json;
using GFDF.Infrastruct.Core;
using GFDF.Infrastruct.Storage;
using GFDF.Infrastruct.Impl;
using GFDP.Sys;
using System.Web.Hosting;

namespace GFDP
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// 
        /// </summary>
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            GFContext.logger = new GFDF.Infrastruct.Impl.LogHelper();
            GFContext.idworker = new IdWorker(2);
            GFContext.repository = new Repository();
            GFContext.cache = new CacheService();
            GFContext.jwt = new JWTHelper("ViIjoiSldUIiwiYXVkIjoiVVNFUiIsImlhdCI6IjIwMjAvNi8yNiA3OjI2OjQwIiwiZGF0YSI6eyJpZCI6MTAwOTl9fQ");
            //Config 配置获取    一是数据库配置，  一是可从数据库中获取的配置
            GFContext.storage = new LocalStorageProvider(HostingEnvironment.MapPath("/"), "/");

            EventBus.idWorker = GFContext.idworker;

            GFContext.load();
            GFContext.logger.Info("Application_Start");
            EventBus.Subscribe(obj =>
            {
                GFContext.logger.Info(JsonConvert.SerializeObject(obj));
            });

            //使用Autofac注册实现类
        }
        /// <summary>
        /// 
        /// </summary>
        protected void Application_End()
        {
            GFContext.logger.Info("Application_End");
        }
    }
}
