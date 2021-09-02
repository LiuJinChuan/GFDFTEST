using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Sys.Entity
{  
	[Table("sys_job")]
    public class JobEntity : BaseEntity
    {		
	    /// <summary>
        /// 
        /// </summary> 
	    public string cname { get; set; } = "";

        /// <summary>
        /// 
        /// </summary> 
        public string callcode { get; set; } = "";

        /// <summary>
        /// 
        /// </summary> 
        public int seqno { get; set; }
	  	  		
	    /// <summary>
        /// 
        /// </summary> 
	    public long deptid { get; set; }
    }
}
 