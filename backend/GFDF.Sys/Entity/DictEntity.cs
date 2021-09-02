using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Sys.Entity
{  
	[Table("sys_dict")]
    public class DictEntity : BaseEntity
    {
	    /// <summary>
        /// 
        /// </summary> 
	    public string cname { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public long pid { get; set; }
	    /// <summary>
        /// 
        /// </summary> 
	    public int seqno { get; set; }
	  	  		
	    /// <summary>
        /// 
        /// </summary> 
	    public string memo { get; set; } = "";

        /// <summary>
        /// 
        /// </summary> 
        public string callcode { get; set; } = "";

        /// <summary>
        /// 
        /// </summary> 
        public string ccolor { get; set; } = "";

        /// <summary>
        /// 
        /// </summary> 
        public string cvalue { get; set; } = "";
    }
}
 