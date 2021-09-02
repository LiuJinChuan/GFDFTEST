using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Sys.Entity
{
    [Table("sys_fin_account")]
    public class Fin_AccountEntity : BaseEntity
    {
        /// <summary>
        /// 账户所属用户
        /// </summary>
        public long custid { get; set; }


        /// <summary>
        /// 账户类型，如积分、储值、收益等
        /// </summary>
        public string atype { get; set; }


        /// <summary>
        /// 总值
        /// </summary>
        public decimal amount_total { get; set; } = 0;


        /// <summary>
        /// 锁定
        /// </summary>
        public decimal amount_lock { get; set; } = 0;


        /// <summary>
        /// 可用
        /// </summary>
        public decimal amount_usable { get; set; } = 0;


        /// <summary>
        /// 创建时间
        /// </summary>
        public long createtime { get; set; } = 0;


        /// <summary>
        /// 备注
        /// </summary>
        public string memo { get; set; } = "";
    }
}
