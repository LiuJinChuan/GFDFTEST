using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Sys.Entity
{  
	[Table("sys_role")]
    public class RoleEntity : BaseEntity
    {      	  		
	    /// <summary>
        /// 
        /// </summary> 
	    public string cname { get; set; } = "";

        /// <summary>
        /// 
        /// </summary> 
        public byte level { get; set; }
	  	  		
	    /// <summary>
        /// 
        /// </summary> 
	    public byte datascope { get; set; }
	  	  		
	    /// <summary>
        /// 
        /// </summary> 
	    public string datavalue { get; set; } = "";

        /// <summary>
        /// 
        /// </summary> 
        public int seqno { get; set; }
	  	  		
	    /// <summary>
        /// 
        /// </summary> 
	    public string memo { get; set; } = "";
    }
}
 