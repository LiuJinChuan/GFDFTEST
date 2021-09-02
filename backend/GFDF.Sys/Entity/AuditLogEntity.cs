using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Sys.Entity
{
    [Table("base_auditlog")]
    public class AuditLogEntity : BaseEntity
    {
        /// <summary>
        /// �û�id
        /// </summary>
        public string userid { get; set; } = "";


        /// <summary>
        /// ������
        /// </summary>
        public string controller { get; set; } = "";


        /// <summary>
        /// ��Ϊ
        /// </summary>
        public string action { get; set; } = "";


        /// <summary>
        /// ����
        /// </summary>
        public string parameters { get; set; } = "";


        /// <summary>
        /// �쳣
        /// </summary>
        public string exception { get; set; } = "";


        /// <summary>
        /// useragent
        /// </summary>
        public string useragent { get; set; } = "";


        /// <summary>
        /// ���
        /// </summary>
        public string result { get; set; } = "";


        /// <summary>
        /// ip
        /// </summary>
        public string ip { get; set; } = "";


        /// <summary>
        /// ����
        /// </summary>
        public string host { get; set; } = "";


        /// <summary>
        /// ����ʱ��
        /// </summary>
        public long duration { get; set; }


        /// <summary>
        /// ����ʱ��
        /// </summary>
        [Write(false)]
        public long createtime => GFContext.idworker.GetTime(id);
    }
}
