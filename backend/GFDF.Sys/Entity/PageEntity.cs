using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Sys.Entity
{  
	[Table("sys_page")]
    public class PageEntity : BaseEntity
    {
	    /// <summary>
        /// 
        /// </summary> 
	    public string cname { get; set; } = "";

        //Ä£¿é
        public string module { get; set; }
        	  	  		
	    /// <summary>
        /// 
        /// </summary> 
	    public int seqno { get; set; }
	  	  		
	    /// <summary>
        /// 
        /// </summary> 
	    public string icon { get; set; } = "";

        /// <summary>
        /// 
        /// </summary> 
        public string style { get; set; } = "";

        /// <summary>
        /// 
        /// </summary> 
        public string link { get; set; } = "";

        /// <summary>
        /// 
        /// </summary> 
        public string component { get; set; } = "";

        /// <summary>
        /// 
        /// </summary> 
        public string callcode { get; set; } = "";
    }
}
 