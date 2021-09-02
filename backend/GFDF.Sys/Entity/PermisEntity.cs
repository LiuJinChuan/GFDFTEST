using System;
using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Sys.Entity
{  
	[Table("sys_permission")]
    public class PermisEntity : BaseEntity
    {
        /// <summary>
        /// 
        /// </summary> 
        public long role { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string webapi { get; set; } = "";
        /// <summary>
        /// 
        /// </summary> 
        public string menu { get; set; } = "";

        /// <summary>
        /// 
        /// </summary> 
        public string page { get; set; } = "";

        /// <summary>
        /// 
        /// </summary> 
        public string button { get; set; } = "";

    }
}
 