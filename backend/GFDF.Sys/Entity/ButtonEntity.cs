using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Sys.Entity
{
    [Table("sys_button")]
    public class ButtonEntity : BaseEntity
    {
        /// <summary>
        /// 
        /// </summary> 
        public string cname { get; set; } = "";

        /// <summary>
        /// 
        /// </summary> 
        public long page { get; set; }
        
        /// <summary>
        /// 
        /// </summary> 
        public string webapi { get; set; }

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
        public string callcode { get; set; } = "";

        /// <summary>
        /// Ö´ÐÐ½Å±¾
        /// </summary> 
        public string script { get; set; } = "";
    }
}
