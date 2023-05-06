using Entity.Base;
using System.ComponentModel;

namespace Entity.entity
{
    public class ProductionRecord : BaseDal
    {
        public string ProductId;
        /// <summary>
        /// 产品名称
        /// </summary>
        [Description("产品名称")]
        public string ProductName;
        /// <summary>
        /// 规格
        /// </summary>
        [Description("规格")]
        public string ProductSpec;

        public string ProductSpecId;

        /// <summary>
        /// 班组
        /// </summary>
        [Description("班组")]
        public string Team;

        public string TeamId;

        /// <summary>
        /// 日期
        /// </summary>
        [Description("日期")]
        public string CreateTime;

        public string Batch;

        /// <summary>
        /// 上传成功数
        /// </summary>
        [Description("上传成功次数")]
        public int SuccessfulNumber;


        public string UpdateId;
    }
}
