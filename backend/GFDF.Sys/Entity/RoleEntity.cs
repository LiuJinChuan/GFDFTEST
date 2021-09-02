using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Sys.Entity
{
    [Table("sys_role")]
    public class RoleEntity : BaseEntity
    {
        /// <summary>
        /// 角色名
        /// </summary> 
        public string cname { get; set; } = "";


        /// <summary>
        /// 角色等级
        /// </summary> 
        public byte level { get; set; }


        /// <summary>
        /// 数据范围
        /// </summary> 
        public byte datascope { get; set; }


        /// <summary>
        /// 数据值
        /// </summary> 
        public string datavalue { get; set; } = "";


        /// <summary>
        /// 排序
        /// </summary> 
        public int seqno { get; set; }


        /// <summary>
        /// 备注
        /// </summary> 
        public string memo { get; set; } = "";
    }
}
