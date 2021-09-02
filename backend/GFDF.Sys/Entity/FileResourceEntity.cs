using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Sys.Entity
{
    [Table("base_fileresource")]
    public class FileResourceEntity : BaseEntity
    {
        /// <summary>
        /// 原始名称
        /// </summary>
        public string orgname { get; set; } = "";


        /// <summary>
        /// 新名称
        /// </summary>
        public string newname { get; set; } = "";


        /// <summary>
        /// Url
        /// </summary>
        public string url { get; set; } = "";


        /// <summary>
        /// 文件类型
        /// </summary>
        public string filetype { get; set; } = "";


        /// <summary>
        /// 创建者
        /// </summary>
        public long creator { get; set; }


        /// <summary>
        /// 文件大小
        /// </summary>
        public int length { get; set; }


        /// <summary>
        /// 内容类型MIME
        /// </summary>
        public string contenttype { get; set; } = "";


        /// <summary>
        /// 存储容器
        /// </summary>
        public string container { get; set; } = "";


        /// <summary>
        /// 内容md5值
        /// </summary>
        public string contentmd5 { get; set; } = "";


        /// <summary>
        /// Etag
        /// </summary>
        public string etag { get; set; } = "";


        /// <summary>
        /// 文件最后修改日期
        /// </summary>
        public long lastmodified { get; set; }
    }
}
