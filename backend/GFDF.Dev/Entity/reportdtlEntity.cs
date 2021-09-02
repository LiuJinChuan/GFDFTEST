using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Dev.Entity
{
    [Table("sys_reportdtl")]
    public class ReportdtlEntity : BaseEntity
    {
        //名称
        public string cname { get; set; } = "";
        
        //报表
        public long reportid { get; set; }

        //表单配置
        public string form { get; set; } = "";
        
        //备注
        public string memo { get; set; } = "";

    }
}
