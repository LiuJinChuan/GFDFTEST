using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Sys.Entity
{
    [Table("sys_notice")]
    public class NoticeEntity : BaseEntity
    {
        /// <summary>
        /// 标题
        /// </summary> 
        public string title { get; set; } = "";


        /// <summary>
        /// 类型
        /// </summary> 
        public string cid { get; set; } = "";


        /// <summary>
        /// 排序
        /// </summary> 
        public int seqno { get; set; }


        /// <summary>
        /// 内容（可以富文本）
        /// </summary> 
        public string content { get; set; } = "";


        /// <summary>
        /// 附加
        /// </summary> 
        public string attachment { get; set; } = "";


        /// <summary>
        /// 范围
        /// </summary> 
        public string scope { get; set; } = "";


        [Write(false)]
        public long createtime => GFContext.idworker.GetTime(this.id);
    }
}
