using Entity.Base;

namespace Entity.entity
{
    public class ProductionInfo : BaseDal
    {
        public string ProductId;

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName;

        /// <summary>
        /// 产品规格
        /// </summary>
        public string ProductSpec;

        public string ProductSpecId;

        public string Team;

        public string TeamId;

        public string Batch;
        /// <summary>
        /// 比例
        /// </summary>
        public string Proportion;

        /// <summary>
        /// 小码组成一个中码需要的数量 如20个小码组一个中码
        /// </summary>
        public int Bag;

        /// <summary>
        /// 中码组成一个大码需要的数量 如5个中码组一个大码
        /// </summary>
        public int Box;

        /// <summary>
        /// 默认为1
        /// </summary>
        public int Duo;

        public string PackingDate;

        public string CreateDate;

        public string Remark;

        // public Dictionary<string, List<string>> ParentCodes;

        /// <summary>
        /// 采集方式  0 123级 1 12级别 2 13级 3 手工关联
        /// </summary>
        public int CollectWay;

        /// <summary>
        /// 方式名称
        /// </summary>
        public string WayName;

        public int StartCount;

        public ProductList product { set; get; }
    }

    public class ProductList
    {
        public string createBy { get; set; }
        public string createTime { get; set; }
        public string updateBy { get; set; }
        public string updateTime { get; set; }
        public string id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string unit { get; set; }
        public string productSpecId { get; set; }
        public string productSpecName { get; set; }
        public string photo { get; set; }
        public string productLineId { get; set; }
        public string productLineName { get; set; }
        public string productTypeId { get; set; }
        public string productTypeCode { get; set; }
        public string productTypeName { get; set; }
        public string remark { get; set; }
        public string marketingId { get; set; }
        public string marketingCode { get; set; }
        public string fullName { get; set; }
        public string productCode { get; set; }

        /// <summary>
        /// 小码组成一个中码需要的数量 如20个小码组一个中码
        /// </summary>
        public int bagNum { get; set; }

        /// <summary>
        /// 中码组成一个大码需要的数量 如5个中码组一个大码
        /// </summary>
        public int boxNum { get; set; }

        /// <summary>
        /// 批号
        /// </summary>
        public string Batch { get; set; }

        /// <summary>
        /// 默认为1
        /// </summary>
        public int Duo;
        /// <summary>
        /// 值
        /// </summary>
        public string Id
        {
            get { return this.id; }
        }
        /// <summary>
        /// 显示的文本
        /// </summary>
        public string Text
        {
            get { return this.fullName; }
        }

        public string Code
        {
            get { return this.productCode; }
        }
    }
}
