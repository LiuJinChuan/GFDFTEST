using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Dev.Entity
{
    [Table("sys_meta")]
    public class MetaEntity : BaseEntity
    {
        //元数据名称
        public string cname { get; set; } = "";
        //元数据名称
        public string tname { get; set; } = "";
        //元数据调用名
        public string callcode { get; set; } = "";
        //备注
        public string memo { get; set; } = "";
    }
}
