using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Sys.Entity
{
    [Table("sys_notice")]
    public class NoticeEntity : BaseEntity
    {
        /// <summary>
        /// ����
        /// </summary> 
        public string title { get; set; } = "";

        /// <summary>
        /// ����
        /// </summary> 
        public string cid { get; set; } = "";

        /// <summary>
        /// ����
        /// </summary> 
        public int seqno { get; set; }

        /// <summary>
        /// ���ݣ����Ը��ı���
        /// </summary> 
        public string content { get; set; } = "";

        /// <summary>
        /// 
        /// </summary> 
        public string attachment { get; set; } = "";

        /// <summary>
        /// 
        /// </summary> 
        public string scope { get; set; } = "";

        [Write(false)]
        public long createtime => GFContext.idworker.GetTime(this.id);
    }
}
