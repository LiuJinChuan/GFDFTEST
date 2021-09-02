using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Sys.Entity
{
    [Table("sys_user")]
    public class UserEntity : BaseEntity
    {
        /// <summary>
        /// 账号
        /// </summary> 
        public string account { get; set; } = "";


        /// <summary>
        /// 名称
        /// </summary> 
        public string cname { get; set; } = "";


        /// <summary>
        /// 密码
        /// </summary> 
        public string pwd { get; set; } = "123456";


        /// <summary>
        /// 性别
        /// </summary> 
        public int sex { get; set; }


        /// <summary>
        /// 手机
        /// </summary> 
        public string phone { get; set; } = "";


        /// <summary>
        /// 邮箱
        /// </summary> 
        public string email { get; set; } = "";


        /// <summary>
        /// 部门
        /// </summary> 
        public long dept { get; set; }


        /// <summary>
        /// 
        /// </summary> 
        public long job { get; set; }


        /// <summary>
        /// 角色
        /// </summary> 
        public string roles { get; set; } = "";
    }
}
