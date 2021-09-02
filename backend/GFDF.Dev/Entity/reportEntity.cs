using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Dev.Entity
{
    [Table("sys_report")]
    public class ReportEntity : BaseEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string cname { get; set; } = "";


        /// <summary>
        /// 调用名称
        /// </summary>
        public string ename { get; set; } = "";


        /// <summary>
        /// 报表类型
        /// </summary>
        public string reporttype { get; set; } = "";


        /// <summary>
        /// 备注
        /// </summary>
        public string memo { get; set; } = "";
    }
}
