using GFDF.Infrastruct.Core;
using GFDF.Infrastruct.Extensions;
using GFDP.Sys.Entity;
using GFDP.Sys.Filter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;

namespace GFDP.Sys.Controllers
{
    [RoutePrefix("common")]
    public class commonController : ApiController
    {
        /// <summary>
        /// 上传文件
        /// </summary>
        [HttpPost, Route("upload")]
        public ResponseResult UploadFile()
        {
            string now = $"{DateTime.Now:yyyyMMdd}";
            var file = HttpContext.Current.Request.Files["files"];
            var tmpfilename = GFContext.idworker.nextId().ToString() + Path.GetExtension(file.FileName);
            GFContext.storage.SaveBlobStream($"Upload/{now}", tmpfilename, file.InputStream).Wait();
            var fileInfo = GFContext.storage.GetBlobFileInfo($"Upload/{now}", tmpfilename).Result;
            var upload = new
            {
                orgname = file.FileName,
                newname = fileInfo.Name,
                url = fileInfo.Url,
                creator = Request.Properties["U_ID"],
                length = file.ContentLength,
                contenttype = file.ContentType,
                container = fileInfo.Container,
                contentmd5 = fileInfo.ContentMD5,
                etag = fileInfo.ETag,
                lastmodified = (fileInfo.LastModified?.GetTimeStamp()) ?? 0L
            };

            EventBus.Emit("Common.Upload", upload);
            ResponseResult result = ResponseResult.Success("上传成功");
            result.data = new { orgname = file.FileName, newname = fileInfo.Name, Url = Request.RequestUri.Scheme + "://" + Request.RequestUri.Host + fileInfo.Url, contenttype = file.ContentType, length = file.ContentLength, lastmodified = (fileInfo.LastModified?.GetTimeStamp()) ?? 0L };
            return result;
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("uploadimg"), AllowAnonymous]
        public ResponseResult UploadImg()
        {
            string now = $"{DateTime.Now:yyyyMMdd}";
            var file = HttpContext.Current.Request.Files["files"];
            var tmpfilename = GFContext.idworker.nextId().ToString() + (string.IsNullOrEmpty(Path.GetExtension(file.FileName)) ? ".png" : Path.GetExtension(file.FileName));
            GFContext.storage.SaveBlobStream($"Upload/{now}", tmpfilename, file.InputStream).Wait();
            var fileInfo = GFContext.storage.GetBlobFileInfo($"Upload/{now}", tmpfilename).Result;
            ResponseResult result = ResponseResult.Success("上传成功");
            result.data = fileInfo.Url;
            return result;
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [Route("filedown"), HttpGet, AllowAnonymous]
        public HttpResponseMessage GetFileFromWebApi(string path)
        {
            try
            {
                FileResourceEntity fileResourceEntity = GFContext.repository.Query<FileResourceEntity>($"select * from @tableName@ where newname='{Path.GetFileName(path)}' and url='{path}'").FirstOrDefault();
                if (fileResourceEntity == null) return new HttpResponseMessage(HttpStatusCode.NoContent);
                var FilePath = System.Web.Hosting.HostingEnvironment.MapPath(path);
                var stream = new FileStream(FilePath, FileMode.Open);
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StreamContent(stream);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = fileResourceEntity.orgname
                };
                return response;
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }
        }

        #region csv导入
        /// <summary>
        /// 步凑一
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("step1")]
        public ResponseResult StepUp1()
        {

            return ResponseResult.Success();
        }

        /// <summary>
        /// 步凑二
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("step2")]
        public ResponseResult StepUp2()
        {

            return ResponseResult.Success();
        }

        /// <summary>
        /// 步凑三
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("step3")]
        public ResponseResult StepUp3()
        {

            return ResponseResult.Success();
        }

        /// <summary>
        /// csv导入
        /// </summary>
        /// <param name="context"></param>
        private void CsvSqlImport(HttpContext context)
        {
            //    if (currUser == null) throw new BaseException(ExEnum.AccessDeniedEx, "该账户已过期请重新登录！");
            //    string path = GFRequest.GetString("path");
            //    string code = GFRequest.GetString("code");
            //    if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(code)) throw new BaseException(ExEnum.ArgNullEx, "参数不能为空或参数不正确！");
            //    path = AppDomain.CurrentDomain.BaseDirectory + path;
            //    if (!File.Exists(path)) throw new BaseException(ExEnum.Other, $"文件不存在{path}！");
            //    var model = BLL.YS.DbConnHelper.GetModelDic("code", code, "exportscript", conn);
            //    if (model == null) throw new BaseException(ExEnum.Other, "数据获取失败！");
            //    //DataTable dataTable = GF.Core.GfExtend.OpenCsv(path);
            //    DataTable dataTable = path.ToLower().Contains(".csv") ? GF.Core.GfExtend.OpenCsv(path) : GF.Core.GfExtend.GetDataTable(path);
            //    //dataTable = GF.Core.GfExtend.OpenCSV(path);
            //    var list = JsonHelper.DataTableToListObj(dataTable);
            //    if (list.Count <= 0) throw new BaseException(ExEnum.Other, "数据为空！");
            //    List<object> vs = new List<object>();
            //    List<string> vss = new List<string>();
            //    for (int i = 0; i < list[0].Keys.Count; i++) vs.Add("col" + (i + 1));
            //    for (int i = 0; i < 200; i++) vss.Add($"[col{i + 1}] varchar(1000) NULL");
            //    string tempTableName = "#" + DateTime.Now.ToString("yyyyMMddHHmmsss");
            //    string sss = GF.Core.GfExtend.StringBuilderJoin(vs, ",");
            //    StringBuilder builder = new StringBuilder();
            //    builder.Append($"CREATE TABLE {tempTableName}({string.Join(",", vss)});");
            //    foreach (var item in list)
            //    {
            //        var vs1 = item.Values.ToList<object>();
            //        bool tf = false;
            //        foreach (var s in vs1)
            //        {
            //            tf = !string.IsNullOrEmpty(s.ToString());
            //            if (tf) break;
            //        }
            //        if (!tf) continue;
            //        string bbb = GF.Core.GfExtend.ArrayToChar(vs1, "'", ",");
            //        builder.Append($"insert into {tempTableName}({sss}) values({bbb});");
            //    }
            //    builder.Append(model["sql"].Replace("@tableName@", tempTableName));
            //    var result = BLL.YS.DbConnHelper.ExecuteSqlTransaction(builder.ToString(), conn);
            //    if (result <= 0) throw new BaseException(ExEnum.Other, "导入失败！");
            //    AJAX_RETURN(context, list);
        }
        #endregion
    }
}