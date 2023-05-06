namespace Server.entity
{
    public class ProductSpecEntity
    {
        public string id;
        public string spec;
        public string specificationsName;
        public string createBy;
        public string createTime;
        public string updateBy;
        public string updateTime;
        public string remark;
        public string delFlag;
        public string productId;

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
            get { return this.specificationsName; }
        }

        public string TextZyy
        {
            get { return this.spec; }
        }
    }
}
