using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Dev.Entity
{
    [Table("sys_db")]
    public class DBEntity : BaseEntity
    {
        /// <summary>
        /// ������Ӣ�ģ�
        /// </summary> 
        public string ename { get; set; }

        /// <summary>
        /// ���������ģ�
        /// </summary> 
        public string cname { get; set; } = "";

        /// <summary>
        /// ������
        /// </summary> 
        public string ctype { get; set; }

        /// <summary>
        /// �����ַ���
        /// </summary> 
        public string connstr { get; set; }

        /// <summary>
        /// ����������
        /// </summary> 
        public long btserver { get; set; } = 0;
    }
}
