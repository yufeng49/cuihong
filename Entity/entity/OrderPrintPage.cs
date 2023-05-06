namespace Entity.entity
{
    public class OrderPrintPage
    {
        private string updateBy;
        private string updateTime;
        private string createBy;
        private string createTime;
        private string remark;
        private string finalPrice;
        private string pricing;
        private string residueMinWarpNum;
        private string residueMaxWarpNum;
        private string valuationUnitsNum;
        private string sellUnitsNum;
        private string orderBagNum;
        private string sellDepartment;
        private string marketingNum;

        private string wrapRatio;
        private string materialName;
        private string materialNum;
        private string customerAddr;
        private string customerDeliveryDate;
        private string orderDate;
        private string status;
        private string orderNum;
        private string customerName;
        private string customerLabel;
        private string orderStatus;
        private string marketingAmount;
        private string belongKey;
        private string orderId;
        private string inStorageNum;
        private string printNum;
        private string orderStatusDesc;
        private bool choose = false;
        private string marketing;
        public string id;
        public bool Choose { get => choose; set => choose = value; }
        public string CreateTime { get => createTime; set => createTime = value; }
        public string OrderNum { get => orderNum; set => orderNum = value; }
        public string CustomerName { get => customerName; set => customerName = value; }
        public string CustomerAddr { get => customerAddr; set => customerAddr = value; }
        public string MaterialName { get => materialName; set => materialName = value; }
        public string SellUnitsNum { get => sellUnitsNum; set => sellUnitsNum = value; }
        public string PrintNum { get => printNum; set => printNum = value; }
        public string InStorageNum { get => inStorageNum; set => inStorageNum = value; }
        public string Marketing { get => marketing; set => marketing = value; }


    }
}
