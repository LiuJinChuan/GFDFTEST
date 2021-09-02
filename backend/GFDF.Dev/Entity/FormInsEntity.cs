using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Dev.Entity
{
    [Table("sys_formins")]
    public class FormInsEntity : BaseEntity
    {
        /// <summary>
        /// 
        /// </summary> 
        public string cname { get; set; } = "";

        /// <summary>
        /// 
        /// </summary> 
        public string bno { get; set; } = "";

        /// <summary>
        /// 
        /// </summary> 
        public string callcode { get; set; } = "";

        /// <summary>
        /// 
        /// </summary>
        public long formid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long creator { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string scopecfg { get; set; } = "";

        /// <summary>
        /// 
        /// </summary> 
        public string memo { get; set; } = "";

    }
}
