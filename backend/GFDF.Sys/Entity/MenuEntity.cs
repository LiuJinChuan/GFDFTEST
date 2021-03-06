using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Sys.Entity
{
    [Table("sys_menu")]
    public class MenuEntity : BaseEntity
    {
        /// <summary>
        /// 菜单名称
        /// </summary> 
        public string cname { get; set; } = "";


        /// <summary>
        /// 菜单类型
        /// </summary> 
        public byte type { get; set; }


        /// <summary>
        /// 上级
        /// </summary> 
        public long pid { get; set; }


        /// <summary>
        /// 排序
        /// </summary> 
        public int seqno { get; set; }


        /// <summary>
        /// 图标
        /// </summary> 
        public string icon { get; set; } = "";


        /// <summary>
        /// 样式
        /// </summary> 
        public string style { get; set; } = "";


        /// <summary>
        /// 链接
        /// </summary> 
        public string link { get; set; } = "";


        /// <summary>
        /// 组件
        /// </summary> 
        public string component { get; set; } = "";


        /// <summary>
        /// 调用标识
        /// </summary> 
        public string callcode { get; set; } = "";
    }
}
