using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Sys.Entity
{
    [Table("adv_advert_release")]
    public class Advert_ReleaseEntity : BaseEntity
    {
        /// <summary>
        /// 所属广告位
        /// </summary>
        public long advertsid { get; set; } = 0;


        /// <summary>
        /// 所属广告
        /// </summary>
        public long advertid { get; set; } = 0;


        /// <summary>
        /// 发布范围
        /// </summary>
        public string range { get; set; } = "";


        /// <summary>
        /// 运行开始时间
        /// </summary>
        public long run_stime { get; set; } = 1;


        /// <summary>
        /// 运行结束时间
        /// </summary>
        public long run_etime { get; set; } = 1;


        /// <summary>
        /// 发布者
        /// </summary>
        public long userid { get; set; } = 0;


        /// <summary>
        /// 创建时间
        /// </summary>
        public long createtime => GFContext.idworker.GetTime(id);
    }
}
