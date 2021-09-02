using System;
using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Sys.Entity
{
    [Table("sys_permission")]
    public class PermisEntity : BaseEntity
    {
        /// <summary>
        /// ��ɫ
        /// </summary> 
        public long role { get; set; }


        /// <summary>
        /// �ӿ�
        /// </summary> 
        public string webapi { get; set; } = "";


        /// <summary>
        /// �˵�
        /// </summary> 
        public string menu { get; set; } = "";


        /// <summary>
        /// ҳ��
        /// </summary> 
        public string page { get; set; } = "";


        /// <summary>
        /// ��ť
        /// </summary> 
        public string button { get; set; } = "";
    }
}
