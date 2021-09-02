using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Sys.Entity
{
    [Table("adv_advert")]
    public class AdvertEntity : BaseEntity
    {
        /// <summary>
        /// 广告名称
        /// </summary>
        public string cname { get; set; } = "";


        /// <summary>
        /// 广告商
        /// </summary>
        public string advertisers { get; set; } = "";


        /// <summary>
        /// 联系电话
        /// </summary>
        public string telephone { get; set; } = "";


        /// <summary>
        /// 链接网址
        /// </summary>
        public string clink { get; set; } = "";


        /// <summary>
        /// 文件地址
        /// </summary>
        public string cpath { get; set; } = "";


        /// <summary>
        /// 广告类型
        /// </summary>
        public string ctype { get; set; } = "";


        /// <summary>
        /// 排序
        /// </summary>
        public long sort_id { get; set; } = 1;


        /// <summary>
        /// 创建时间
        /// </summary>
        public long createtime => GFContext.idworker.GetTime(id);
    }
}
