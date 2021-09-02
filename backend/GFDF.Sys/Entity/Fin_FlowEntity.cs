using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Sys.Entity
{
    [Table("sys_fin_flow")]
    public class FINFlowEntity : BaseEntity
    {
        //所属账户
        public long accountid { get; set; } = 0;

        //账户所属用户
        public long custid { get; set; }
        //账户类型： USDT  USDT奖励 ETH
        public string atype { get; set; }
        //业务名称：   如提现/充值  现金红包。   业务类型就象 现金红包下有 返还现金红包（超时未领取）、 支付***

        //业务类型: 提现-USDT奖励 提现-ETH 充值USDT 积分消耗等
        public string btype { get; set; }

        //收支类型 收、支
        public string direction { get; set; }

        //发生金额
        public decimal amount { get; set; }

        //财户余额
        public decimal balance { get; set; } = 0;

        //关联业务
        public long bno { get; set; }

        //记账时间
        public long createtime { get; set; } = 0;

        //备注
        public string memo { get; set; } = "";
    }
}
