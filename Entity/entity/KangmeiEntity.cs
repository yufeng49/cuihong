using Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.entity
{
   public class KangmeiEntity  : BaseDal
    {
        public string productCode { get; set; }
        public string subTypeNo { get; set; }
        public string productName { get; set; }
        public string subType { get; set; }
        public string comment { get; set; }
        public string spec { get; set; }
        public string packageSpec { get; set; }
        public List<KangmeiSpec> resProdCodeList { get; set; }
    }
    public class ProdInfo : BaseDal
    {
        public string ProductName;
        public string SubType;
        public string SubTypeno;
        public string SpecDesc;
        public string PackageSpec;
        /// <summary>
        /// 识别码codeVersion+value
        /// </summary>
        public string VerfyCode;
        /// <summary>
        /// 包装比例 格式1:2
        /// </summary>
        public string PkgRatio;
        /// <summary>
        /// 相机单次采集数量
        /// </summary>
        public string CollectCount;
        /// <summary>
        /// 产品id
        /// </summary>
        public string ProdId;
    }
    public class KangmeiSpec {
        /// <summary>
        /// 识别码 codeVersion+value
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 包装比例
        /// </summary>
        public string pkgRatio { get; set; }
        /// <summary>
        /// 级别1.小码 2.大码
        /// </summary>
        public string codeLevel { get; set; }
        /// <summary>
        /// 识别码codeVersion+value
        /// </summary>
        public string codeVersion { get; set; }
    }
}
