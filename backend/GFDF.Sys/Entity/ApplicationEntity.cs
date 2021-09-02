using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Sys.Entity
{
    [Table("sys_application")]
    public class ApplicationEntity : BaseEntity
    {
        /// <summary>
        /// Ӧ������
        /// </summary> 
        public string cname { get; set; }

        /// <summary>
        /// Ӧ�ñ�ʶ
        /// </summary> 
        public string appsecret { get; set; }

        /// <summary>
        /// Ӧ������
        /// </summary> 
        public string description { get; set; } = "";

        /// <summary>
        /// Ӧ��ͼ��
        /// </summary> 
        public string icon { get; set; } = "";
    }
}
