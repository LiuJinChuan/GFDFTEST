using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Dev.Entity
{
    [Table("sys_reportdtl")]
    public class ReportdtlEntity : BaseEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string cname { get; set; } = "";


        /// <summary>
        /// 报表
        /// </summary>
        public long reportid { get; set; }


        /// <summary>
        /// 表单配置
        /// </summary>
        public string form { get; set; } = "";


        /// <summary>
        /// 备注
        /// </summary>
        public string memo { get; set; } = "";
    }
}
