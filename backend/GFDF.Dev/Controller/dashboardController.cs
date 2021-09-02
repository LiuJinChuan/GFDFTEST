using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using GFDF.Infrastruct.Core;
using GFDP.Dev.Entity;
using GFDP.Sys.Controllers;
using SysContext = GFDP.Sys.GFContext;

namespace GFDP.Dev.Controllers
{
    [RoutePrefix("base/dashboard")]
    public class DashboardController : BaseController<DashboardEntity>
    {
        /// <summary>
        /// 更新仪表盘图表
        /// </summary>
        /// <returns></returns>
        [HttpPost,Route("upchart")]
        public ResponseResult UpCharts(long id, string charts)
        {
            string sql = $@"update base_dashboard set charts={charts} where id={id});";
            SysContext.repository.Execute(sql);
            EventBus.Emit($"AccountController.UpDefault", id);
            return ResponseResult.Success();
        }
        /// <summary>
        /// 仪表盘添加图表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("addchart")]
        public ResponseResult AddCharts(dynamic obj)
        {
            string sql = $@"update base_dashboard set charts=charts+','+cast({obj.chartid.Value} as varchar) where id={obj.dbid.Value};";
            SysContext.repository.Execute(sql);
            EventBus.Emit($"AccountController.UpDefault", obj);
            return ResponseResult.Success();
        }

        /// <summary>
        /// 仪表盘添加图表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("chart/{chartid:long}")]
        public ResponseResult QueryByChart(long chartid)
        {
            string sql = $@"select * from base_dashboard where CHARINDEX(cast({chartid} as varchar),charts)>0;";
            var result = ResponseResult.Success();
            result.data = SysContext.repository.Query<DashboardEntity>(sql);
            return result;
        }
    }
}
