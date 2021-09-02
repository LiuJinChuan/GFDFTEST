using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Sys.Entity
{
    /// <summary>
    /// 多对多关系集中映射
    /// </summary>
    [Table("sys_relevance")]
    public partial class RelevanceEntity : BaseEntity
    {
        /// <summary>
	    /// 描述
	    /// </summary>
        public string description { get; set; } = "";


        /// <summary>
	    /// 映射标识
	    /// </summary>
        public string key { get; set; } = "";


        /// <summary>
	    /// 授权人
	    /// </summary>
        public long actor { get; set; }


        /// <summary>
	    /// 第一个表主键ID
	    /// </summary>
        public long first { get; set; }


        /// <summary>
	    /// 第二个表主键ID
	    /// </summary>
        public long second { get; set; }


        /// <summary>
	    /// 第三个主键
	    /// </summary>
        public long third { get; set; }
    }
}