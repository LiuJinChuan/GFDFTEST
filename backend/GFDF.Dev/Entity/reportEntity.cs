using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Dev.Entity
{
    [Table("sys_report")]
    public class ReportEntity : BaseEntity
    {
        //名称
        public string cname { get; set; } = "";

        //调用名称
        public string ename { get; set; } = "";

        //报表类型
        public string reporttype { get; set; } = "";
        
        //备注
        public string memo { get; set; } = "";
    }
}
