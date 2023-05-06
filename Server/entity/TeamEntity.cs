namespace Server.entity
{
    public class TeamEntity
    {
        public string createBy;
        public string createTime;
        public string updateBy;
        public string updateTime;
        public string remark;
        public string id;
        public string teamName;
        public string delFlag;

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
            get { return this.teamName; }
        }
    }

}
