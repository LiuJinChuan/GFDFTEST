using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Dev.Entity
{
    [Table("sys_codeseg")]
    public class CodeSegEntity : BaseEntity
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
        /// 分类
        /// </summary>
        public string cid { get; set; } = "";


        /// <summary>
        /// 当前编码
        /// </summary>
        public string currcode { get; set; } = "";


        /// <summary>
        /// 标记
        /// </summary>
        public string tags { get; set; } = "";


        /// <summary>
        /// 备注
        /// </summary>
        public string memo { get; set; } = "";
    }
}
