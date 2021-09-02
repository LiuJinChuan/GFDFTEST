using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Dev.Entity
{
    [Table("sys_codesegdtl")]
    public class CodeSegDtlEntity : BaseEntity
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
        /// 脚本id
        /// </summary>
        public long segid { get; set; }


        /// <summary>
        /// 版本
        /// </summary>
        public int ver { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        public string memo { get; set; } = "";
    }
}
