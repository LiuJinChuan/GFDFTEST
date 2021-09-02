using System;
using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Sys.Entity
{
    [Table("sys_permission")]
    public class PermisEntity : BaseEntity
    {
        /// <summary>
        /// 角色
        /// </summary> 
        public long role { get; set; }


        /// <summary>
        /// 接口
        /// </summary> 
        public string webapi { get; set; } = "";


        /// <summary>
        /// 菜单
        /// </summary> 
        public string menu { get; set; } = "";


        /// <summary>
        /// 页面
        /// </summary> 
        public string page { get; set; } = "";


        /// <summary>
        /// 按钮
        /// </summary> 
        public string button { get; set; } = "";
    }
}
