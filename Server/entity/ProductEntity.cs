namespace Server.entity
{
    public class ProductEntity
    {
        public string id;
        public string productName;
        public string productCode;
        public string productPhoto;
        public string createBy;
        public string createTime;
        public string updateBy;
        public string updateTime;
        public string delFlag;
        public string shelfLife;
        public string licenceCode;
        public string description;
        public string remark;

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
            get { return this.productName; }
        }

    }
}
