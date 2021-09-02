using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Sys.Entity
{
    [Table("base_fileresource")]
    public class FileResourceEntity : BaseEntity
    {
        //原始名称
        public string orgname { get; set; } = "";
        //新名称
        public string newname { get; set; } = "";
        //Url
        public string url { get; set; } = "";
        //文件类型
        public string filetype { get; set; } = "";
        //创建者
        public long creator { get; set; }
        //文件大小
        public int length { get; set; }
        //内容类型MIME
        public string contenttype { get; set; } = "";
        //存储容器
        public string container { get; set; } = "";
        //内容md5值
        public string contentmd5 { get; set; } = "";
        //Etag
        public string etag { get; set; } = "";
        //文件最后修改日期
        public long lastmodified { get; set; }

    }

}
