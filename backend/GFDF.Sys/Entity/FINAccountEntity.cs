using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Sys.Entity
{
    [Table("sys_fin_account")]
    public class FINAccountEntity : BaseEntity
    {
        //账户所属用户
        public long custid { get; set; }

        //账户类型，如积分、储值、收益等
        public string atype { get; set; }

        //总值
        public decimal amount_total { get; set; } = 0;

        //锁定
        public decimal amount_lock { get; set; } = 0;

        //可用
        public decimal amount_usable { get; set; } = 0;

        //创建时间
        public long createtime { get; set; } = 0;

        //备注
        public string memo { get; set; } = "";
    }
}
