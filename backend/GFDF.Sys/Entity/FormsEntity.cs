using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Sys.Entity
{
    [Table("sys_forms")]
    public class FormEntity : BaseEntity
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
        public string cid { get; set; } = "";

        /// <summary>
        /// 
        /// </summary> 
        public int ver { get; set; }
        /// <summary>
        /// 
        /// </summary> 
        public string memo { get; set; } = "";
    }
}
