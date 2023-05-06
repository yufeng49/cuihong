using System.Collections.Generic;

namespace Server.entity
{
    public class ReprintPageEntity
    {
        public List<ReprintEntity> records;
    }

    public class ReprintEntity
    {
        public string productId;
        public string productId_dictText;
        public string updateTime;
        public string remark;
        public string delFlag;
        public string productName;
        public string productSpecName;
        public string productSpecificationsId;
        public string createBy;
        public string qrCode;
        public string productSpecificationsId_dictText;
        public string createTime;
        public string updateBy;
        public string oneQrCode;
        public string productionBatch;
        public string id;
        public string productSpecificationsName;
        public string batchAccount;
    }
}
