using Entity.Base;
using MongoDB.Bson;

namespace Entity.entity
{
    public class PrintConfig : BaseDal
    {
        public string PrintType;

        public string PrintName;

        public int FontX;

        public int FontY;

        public int CodeX;

        public int CodeY;

        public string Com;

        public string BaudRate;

        public int DefaultPrint;

        public int CodeSize;
    }


    public class BaseConfig : BaseDal
    {
        /// <summary>
        /// 键
        /// </summary>
        public string Bckey;
        /// <summary>
        /// 值
        /// </summary>
        public string Bcvalue;
        /// <summary>
        /// 状态 1.启用 2.停用
        /// </summary>
        public int Bcstatus;
    }


    /// <summary>
    /// 每个采集的小码信息，采集到第一个时 生成一个随机箱码和报工单号，到组箱时 获取箱码再做更改
    /// </summary>
    public class km_CollectCode
    {

        /// <summary>
        /// 标识
        /// </summary>
        public ObjectId id { get; set; }

        /// <summary>
        /// 报工单号
        /// </summary>
        public string UploadOrder { get; set; }
        /// <summary>
        /// 采集时间
        /// </summary>
        public string collectime { get; set; }
        /// <summary>
        /// 采集的小码
        /// </summary>
        public string lcode1 { get; set; }
        /// <summary>
        /// 随机生成的件码
        /// </summary>
        public string lcode2 { get; set; }
        /// <summary>
        /// 箱码
        /// </summary>
        public string lcode3 { get; set; }
        /// <summary>
        /// 每个小码的批次号(每箱或者每件的批号不一样，则为混批)
        /// </summary>
        public string batch { get; set; }
        /// <summary>
        /// 上传状态 0.未上传 1.已装箱  2已上传
        /// </summary>
        public int upstatus { get; set; }
    }
    /// <summary>
    /// 每次生产时的上传记录
    /// </summary>
    public class km_UploadInfo
    {
        /// <summary>
        /// 标识
        /// </summary>
        public ObjectId id { get; set; }

        /// <summary>
        /// 报工单号
        /// </summary>
        public string UploadOrder { get; set; }

        /// <summary>
        /// 产品id
        /// </summary>
        public string proidid { get; set; }
        /// <summary>
        /// 包装日期
        /// </summary>
        public string madedate { get; set; }
        /// <summary>
        /// 比例 比如：1:200
        /// </summary>
        public string prop { get; set; }
        /// <summary>
        /// 生产日期
        /// </summary>
        public string proddate { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public string expiredate { get; set; }
        /// <summary>
        /// 产线编码
        /// </summary>
        public string cxid { get; set; }
        /// <summary>
        /// 产线负责人
        /// </summary>
        public string cxmanager { get; set; }
        /// <summary>
        /// 工厂
        /// </summary>
        public string factoryid { get; set; }
        /// <summary>
        /// 上传状态 0.未上传 1.已上传
        /// </summary>
        public int upstatus { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string creatime { get; set; }
    }
    public class ch_Uploadstr
    {

        public ObjectId id { get; set; }
        public string creatime { get; set; }
 
        ///<summary>
        ///对应ch_collect的parentcode ,上传成功后可作为删除条件，并且修改采集表的状态
        /// </summary>
        public string fcode { get; set; }

       
        /// <summary>
        /// 上传字符串
        /// </summary>
        public string uploadstr { get; set; }

    }
    public class ch_Collect
    {
        public ObjectId id { get; set; }
        /// <summary>
        /// 采集的袋码或者箱码
        /// </summary>
       public string  code { get; set; }
        /// <summary>
        /// 父级码
        /// </summary>
        public string parentcode { get; set; }
        /// <summary>
        /// 码类型 1代表袋箱  2代表箱垛
        /// </summary>
        public int codelevel { get; set; }
        /// <summary>
        ///    采集时间
        /// </summary>
        public string collectime { get; set; }
        /// <summary>
        /// 码状态  1.已采集  2已上传
        /// </summary>
        public int codestatus { get; set; }
    }
}
