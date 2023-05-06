using System.Collections.Generic;

namespace Entity.entity
{
    public class TrackWay
    {
        /// <summary>
        /// 轨道名称
        /// </summary>
        public string TrackWayName { get; set; }
        /// <summary>
        /// 码类型 1,一物一码 2.公众号 3.西核码
        /// </summary>
        public int CodeType { get; set; }
        /// <summary>
        /// 产品集合
        /// </summary>
        public List<ListItem> ls { get; set; }
        /// <summary>
        /// 选择的产品
        /// </summary>
        public ProductList SingleItem { get; set; }
        /// <summary>
        /// 批次
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// 生产日期
        /// </summary>
        public string ProductionDate { get; set; }
        /// <summary>
        /// 包装比例比例
        /// </summary>
        public int PackingRatio { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsUse { get; set; }


        /// <summary>
        /// 容错数量
        /// </summary>
        public int rcnum { get; set; }

        /// <summary>
        /// 已采集的数据，组垛成功后清空
        /// </summary>
        public List<string> CollectList { get; set; }
    }
    public class ProductInfo
    {
        private string id;
        /// <summary>
        /// 产品名称
        /// </summary>
        private string name;
        /// <summary>
        /// 产品编码
        /// </summary>
        public string materialNo;

        public string lpName;

        public string lpCode;

        public string Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string LpName { get => lpName; set => lpName = value; }
    }

    public class ListItem
    {
        private string m_sValue = string.Empty;
        private string m_sText = string.Empty;

        /// <summary>
        /// 值
        /// </summary>
        public string Value
        {
            get { return this.m_sValue; }
        }
        /// <summary>
        /// 显示的文本
        /// </summary>
        public string Text
        {
            get { return this.m_sText; }
        }

        public string Id;

        public ListItem(string value, string text, string id)
        {
            this.m_sValue = value;
            this.m_sText = text;
            this.Id = id;
        }

    }
}
