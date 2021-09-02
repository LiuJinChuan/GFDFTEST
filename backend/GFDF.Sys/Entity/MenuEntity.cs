using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Sys.Entity
{
    [Table("sys_menu")]
    public class MenuEntity : BaseEntity
    {
        /// <summary>
        /// �˵�����
        /// </summary> 
        public string cname { get; set; } = "";


        /// <summary>
        /// �˵�����
        /// </summary> 
        public byte type { get; set; }


        /// <summary>
        /// �ϼ�
        /// </summary> 
        public long pid { get; set; }


        /// <summary>
        /// ����
        /// </summary> 
        public int seqno { get; set; }


        /// <summary>
        /// ͼ��
        /// </summary> 
        public string icon { get; set; } = "";


        /// <summary>
        /// ��ʽ
        /// </summary> 
        public string style { get; set; } = "";


        /// <summary>
        /// ����
        /// </summary> 
        public string link { get; set; } = "";


        /// <summary>
        /// ���
        /// </summary> 
        public string component { get; set; } = "";


        /// <summary>
        /// ���ñ�ʶ
        /// </summary> 
        public string callcode { get; set; } = "";
    }
}
