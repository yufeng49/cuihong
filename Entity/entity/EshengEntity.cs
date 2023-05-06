using Entity.Base;

namespace Entity.entity
{
    public class EshengEntity : BaseDal
    {
        public string CreateDept;
        public string CreateTime;
        public string CreateTimes;
        public string CreateUser;
        public string Hdno;
        public int IsDeleted;
        public string LeaveFactoryNo;
        public string MaterialId;
        public string MaterialName;
        public string MaterialNo;
        public string QrCode;
        public string QrCodeflow;
        public string RandomCode;
        /// <summary>
        /// 入库状态 0.已拉取，1.已打印 2.已采集 3.已入库 4.采集失败
        /// </summary>
        public string Status;
        public string UpdateTime;
        public string UpdateUser;
        public decimal Weight;
        public string WeightType;

        public string Batch;

        /// <summary>
        /// 打印时间
        /// </summary>
        public string Printtime;
        /// <summary>
        /// 采集时间
        /// </summary>
        public string Collecttime;
        /// <summary>
        /// 入库状态 0.已拉取，1.已打印 2.已采集 3.已入库 4.采集失败
        /// </summary>
        public string Inwarehouse;


    }

    public class EshengCheckout : BaseDal
    {
        public string CarModel;
        public string DispatchOrderId;
        public int DispatchOrderType;
        public string FactoryCode;
        public string FactoryName;
        public string HdNo;
        public string LeaveFactoryNo;

        public string LeaveNum;
        public string LicensePlateNo;
        public string MaterialId;
        public string MaterialName;
        public string MaterialNo;
        public int OutNum;
        public string OutWeight;
        public string TotalWeight;
        /// <summary>
        /// 出库状态 0 已拉取订单 1.出库完成
        /// </summary>
        public int OoutStatus;
        public string OutTime;
        /// <summary>
        /// 单个出库码，逗号隔开
        /// </summary>
        public string Qrcode;
    }
    public class EshengCheckout_detail : BaseDal
    {
        /// <summary>
        /// 出库单
        /// </summary>
        public string outNum { get; set; }
        /// <summary>
        /// 二维码
        /// </summary>
        public string qrCode { get; set; }
        /// <summary>
        /// 写入时间
        /// </summary>
        public string writeDate { get; set; }

        /// <summary>
        /// 状态1.已上传 
        /// </summary>
        public string qrState { get; set; }
        public string leaveFactoryNo { get; set; }
        public string weight { get; set; }
        public string materialNo { get; set; }
        public string materialName { get; set; }
    }

    public class stockOutinfo
    {

        public qrcodeinfo qrcodeinfo;
        public truckingOrder truckingOrder;
    }

    public class qrcodeinfo
    {
        public string createDept;
        public string createTime;
        public string createTimes;

        public string createUser;
        public string hdNo;

        public int isDeleted;
        public string leaveFactoryNo;
        public string materialId;
        public string materialName;
        public string materialNo;
        public string qrCode;
        public string qrCodeFlow;
        public string randomCode;

        public string status;
        public string updateTime;

        public string updateUser;
        public string weight;
        public string weightType;
    }
    public class truckingOrder
    {
        public string carModel;
        public string createDept;
        public string createTime;
        public string createTimes;
        public string createUser;
        public string dispatchOrderId;
        public string dispatchOrderType;
        public object erpError;
        public string factoryCode;
        public string factoryName;
        public string hdNo;
        public int isDeleted;
        public string leaveFactoryNo;
        public string leaveNum;
        public string licensePlateNo;

        public string materialId;

        public string materialName;
        public string materialNo;
        public string outNum;
        public string outStatus;
        public string intoutWeight;
        public string shipmentNo;
        public string status;
        public string totalNum;
        public string totalWeight;
        public string unsArchivesCode;

        public string unsArchivesName;
        public string updateTime;
        public string updateUser;
        public string weightType;
    }
}
