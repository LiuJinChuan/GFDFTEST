using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Sys.Entity
{  
	[Table("sys_param")]
    public class ParamEntity : BaseEntity
    {
      	  		
	    /// <summary>
        /// 
        /// </summary> 
	    public string cname { get; set; } = "";

        /// <summary>
        /// 
        /// </summary> 
        public int seqno { get; set; }
	  	  		
	    /// <summary>
        /// 
        /// </summary> 
	    public string callcode { get; set; } = "";

    }
}
 