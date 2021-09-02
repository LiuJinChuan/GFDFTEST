using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Sys.Entity
{
    [Table("sys_user")]
    public class UserEntity : BaseEntity
    {
        /// <summary>
        /// 
        /// </summary> 
        public string account { get; set; } = "";

        /// <summary>
        /// 
        /// </summary> 
        public string cname { get; set; } = "";

        /// <summary>
        /// 
        /// </summary> 
        public string pwd { get; set; } = "123456";

        /// <summary>
        /// 
        /// </summary> 
        public int sex { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string phone { get; set; } = "";

        /// <summary>
        /// 
        /// </summary> 
        public string email { get; set; } = "";

        /// <summary>
        /// 
        /// </summary> 
        public long dept { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public long job { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string roles { get; set; } = "";

        /// <summary>
        /// 
        /// </summary> 
        //public string bizno { get; set; } = "";
    }
}
