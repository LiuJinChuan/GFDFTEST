using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Dev.Entity
{
    [Table("base_chart")]
    public class ChartEntity : BaseEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string cname { get; set; } = "";


        /// <summary>
        /// 调用标识
        /// </summary>
        public string callcode { get; set; } = "";


        /// <summary>
        /// 备注
        /// </summary>
        public string memo { get; set; }
    }
}
