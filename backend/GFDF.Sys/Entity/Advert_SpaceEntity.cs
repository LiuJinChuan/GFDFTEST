using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Sys.Entity
{
    [Table("adv_advert_space")]
    public class Advert_SpaceEntity : BaseEntity
    {
        /// <summary>
        /// 位置简称
        /// </summary>
        public string cname { get; set; } = "";


        /// <summary>
        /// 位置编号
        /// </summary>
        public string code { get; set; } = "";


        /// <summary>
        /// 位置描述
        /// </summary>
        public string desc { get; set; } = "";


        /// <summary>
        /// 所属平台
        /// </summary>
        public string platform { get; set; } = "";


        /// <summary>
        /// 广告类型
        /// </summary>
        public string ctype { get; set; } = "";


        /// <summary>
        /// 展示类型
        /// </summary>
        public string showtype { get; set; } = "";


        /// <summary>
        /// 排序码
        /// </summary>
        public long sort_id { get; set; } = 1;


        /// <summary>
        /// 创建时间
        /// </summary>
        public long createtime => GFContext.idworker.GetTime(id);
    }
}
