using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Sys.Entity
{
    [Table("sys_button")]
    public class ButtonEntity : BaseEntity
    {
        /// <summary>
        /// 按钮名称
        /// </summary> 
        public string cname { get; set; } = "";


        /// <summary>
        /// 所属页面
        /// </summary> 
        public long page { get; set; }


        /// <summary>
        /// 调用接口
        /// </summary> 
        public string webapi { get; set; }


        /// <summary>
        /// 排序
        /// </summary> 
        public int seqno { get; set; }


        /// <summary>
        /// 图标
        /// </summary> 
        public string icon { get; set; } = "";


        /// <summary>
        /// 调用标识
        /// </summary> 
        public string callcode { get; set; } = "";


        /// <summary>
        /// 执行脚本
        /// </summary> 
        public string script { get; set; } = "";
    }
}
