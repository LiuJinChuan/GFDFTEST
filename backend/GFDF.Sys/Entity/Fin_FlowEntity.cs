using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Sys.Entity
{
    [Table("sys_fin_flow")]
    public class FINFlowEntity : BaseEntity
    {
        /// <summary>
        /// 所属账户
        /// </summary>
        public long accountid { get; set; } = 0;


        /// <summary>
        /// 账户所属用户
        /// </summary>
        public long custid { get; set; }


        /// <summary>
        /// 账户类型
        /// </summary>
        public string atype { get; set; }


        /// <summary>
        /// 业务类型
        /// </summary>
        public string btype { get; set; }


        /// <summary>
        /// 收支类型 收、支
        /// </summary>
        public string direction { get; set; }


        /// <summary>
        /// 发生金额
        /// </summary>
        public decimal amount { get; set; }


        /// <summary>
        /// 财户余额
        /// </summary>
        public decimal balance { get; set; } = 0;


        /// <summary>
        /// 关联业务
        /// </summary>
        public long bno { get; set; }


        /// <summary>
        /// 记账时间
        /// </summary>
        public long createtime { get; set; } = 0;


        /// <summary>
        /// 备注
        /// </summary>
        public string memo { get; set; } = "";
    }
}
