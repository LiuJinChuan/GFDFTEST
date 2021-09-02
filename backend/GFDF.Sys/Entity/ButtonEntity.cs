using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Sys.Entity
{
    [Table("sys_button")]
    public class ButtonEntity : BaseEntity
    {
        /// <summary>
        /// ��ť����
        /// </summary> 
        public string cname { get; set; } = "";


        /// <summary>
        /// ����ҳ��
        /// </summary> 
        public long page { get; set; }


        /// <summary>
        /// ���ýӿ�
        /// </summary> 
        public string webapi { get; set; }


        /// <summary>
        /// ����
        /// </summary> 
        public int seqno { get; set; }


        /// <summary>
        /// ͼ��
        /// </summary> 
        public string icon { get; set; } = "";


        /// <summary>
        /// ���ñ�ʶ
        /// </summary> 
        public string callcode { get; set; } = "";


        /// <summary>
        /// ִ�нű�
        /// </summary> 
        public string script { get; set; } = "";
    }
}
