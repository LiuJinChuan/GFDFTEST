using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using GFDF.Infrastruct.Core;
using GFDP.Dev.Entity;
using GFDP.Sys.Controllers;
using SysContext = GFDP.Sys.GFContext;

namespace GFDP.Dev.Controllers
{
    [RoutePrefix("data/sy_csv")]
    public class Sy_csvController : BaseController<sy_csvEntity>
    {
        /// <summary>
        /// 上传文件
        /// </summary>
        [HttpPost]
        [Route("uploadfile")]
        public ResponseResult UploadFile(long userid)
        {
            string userid1 = HttpContext.Current.Request.Params["userid"];
            string ccontent = HttpContext.Current.Request.Params["ccontent"];
            var name = HttpContext.Current.Request.Files["files"].FileName;
            var file = HttpContext.Current.Request.Files["files"];
            string filePath = "/file/";
            string ab = System.Web.Hosting.HostingEnvironment.MapPath(filePath);
            if (!Directory.Exists(ab)) Directory.CreateDirectory(ab);
            sy_csvEntity csv = new sy_csvEntity { userid = userid };
            var aa = base.Create(csv);
            string cname = aa.NewID + "";
            ab = ab + cname + ".csv";

            file.SaveAs(ab);
            if (!File.Exists(ab)) throw new BaseException(ExEnum.Other, $"上传失败！");
            string sql = $"update data_sy_csv set cname='{name}',csvpath='{filePath + cname + ".csv"}' where id={aa.NewID}";
            SysContext.repository.Execute(sql);
            return ResponseResult.Success("上传成功！");
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        [HttpGet]
        [Route("getcsv/{fileid:long}")]
        public ResponseResult GetCSV(long fileid)
        {
            string filepath = System.Web.Hosting.HostingEnvironment.MapPath("/file/") + fileid.ToString() + ".csv";
            string content = FileEncoding.TryLoadFile(filepath, "");
            ResponseResult result = ResponseResult.Success();
            result.data = content;
            return result;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        [HttpGet]
        [Route("import/{fileid:long}")]
        public ResponseResult ImportCSV(long fileid)
        {
            string filepath = System.Web.Hosting.HostingEnvironment.MapPath("/file/") + fileid.ToString() + ".csv";
            var csvhelper = new GFGXDY.Biz.CSVhelper();
            System.Data.DataTable dt = csvhelper.csv2dt(filepath,0);

            csvhelper.SqlBulkCopyByDatatable("sys_temp", dt);

            //后续处理
            return ResponseResult.Success();
        }
    }
}
