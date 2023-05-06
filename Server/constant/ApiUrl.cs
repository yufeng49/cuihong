namespace Server.constant
{
    public static class ApiUrl
    {
        #region 峨胜水泥

        /// <summary>
        /// 获取入库信息
        /// </summary>
        public const string printQrCode = "/inner/qrCode/printQrCode";//获取产品
        /// <summary>
        /// 确认入库
        /// </summary>
        public const string collect = "/inner/qrCode/collect";
        /// <summary>
        /// 获取出库单
        /// </summary>
        public const string checkout = "/inner/qrCode/checkout";
        /// <summary>
        /// 单个出库
        /// </summary>
        public const string stockOut = "/inner/qrCode/stockOut";
        /// <summary>
        /// 出库确认
        /// </summary>
        public const string submitConfirm = "/inner/qrCode/submitConfirm";
        /// <summary>
        /// 服务器状态检测
        /// </summary>
        public const string ping = "/inner/qrCode/ping";

        /// <summary>
        /// 删除单个出库
        /// </summary>
        public const string delStockOut = "/inner/qrCode/delStockOut";

        /// <summary>
        /// 获取二维码信息
        /// </summary>
        public const string getQrCode = "/inner/qrCode/getQrCode";
        public const string setQrCode = "/inner/qrCode/setQrCode";

        #endregion


        #region 康美
        /// <summary>
        /// 获取产品列表
        /// </summary>
        public const string km_GetProdList = "kangmei-api/client/productList";
        /// <summary>
        /// 获取箱码
        /// </summary>
        public const string GetCode = "kangmei-api/client/getBoxCurCodeByProductId/";


        public const string km_UpLoad = "kangmei-api/client/uploadCode";
        #endregion

        #region 翠红
        /// <summary>
        /// 登录
        /// </summary>
        public const string ch_Login = "/api2/client/index/login";
        /// <summary>
        /// 获取产品
        /// </summary>
        public const string getProduct = "/api2/client/currency/getProduct";
        /// <summary>
        /// 查询产品规格
        /// </summary>
        public const string getProductBasic = "/api2/client/currency/getProductBasic";

        /// <summary>
        /// 获取生产单据号
        /// </summary>
        public const string getBillNo = "/api2/client/currency/getBillNo";

        /// <summary>
        /// 获取生产任务
        /// </summary>
        public const string getTask = "/api2/client/currency/getTask";


        /// <summary>
        /// 获取箱码垛码
        /// </summary>
        public const string getParentQrCode = "/api2/client/currency/getParentQrCode";
        /// <summary>
        /// 上传接口
        /// </summary>
        public const string uploadCode = "/api2/client/code/uploadCode";

        #endregion
    }
}
