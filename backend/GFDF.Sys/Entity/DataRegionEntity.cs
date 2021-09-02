using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Sys.Entity
{
    [Table("data_region")]
    public class DataRegionEntity : BaseEntity
    {
        /// <summary>
        /// 名称
        /// </summary> 
        public string cname { get; set; } = "";

        /// <summary>
        /// 行政代码
        /// </summary> 
        public string code { get; set; }

        /// <summary>
        /// 父级
        /// </summary> 
        public string parentcode { get; set; }

        /// <summary>
        /// 类别
        /// </summary> 
        public string classcode { get; set; } = "";

        /// <summary>
        /// 位置
        /// </summary> 
        public string localtion { get; set; } = "";

        /// <summary>
        /// 位置
        /// </summary> 
        [Write(false)]
        public string level { get; set; } = "0";

        //操作时间
        public long ctime { get; set; } = 0;

        [Write(false)]
        public string jcode { get { return string.IsNullOrEmpty(code) || code == "0" ? "0" : code.TrimEnd('0'); } }

        [Write(false)]
        public string jparentcode { get { return string.IsNullOrEmpty(parentcode) || parentcode == "0" ? "0" : parentcode.TrimEnd('0'); } }
    }
}
