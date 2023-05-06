using BLL.Base;
using Dll.Sub;
using Entity.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Sub
{


    public class ch_CollectBll : BaseServer<ch_Collect>
    {
        private ch_CollectDal ch_Collect;
        public ch_CollectBll()
        {
            ch_Collect = new ch_CollectDal();
        }

        public bool Addch_Collectls(List<ch_Collect> ls)
        {
            return ch_Collect.Addls(ls);
        }
        public List<ch_Uploadstr> GetUploadls()
        {
            return ch_Collect.GetUploadls();
        }

        /// <summary>
        /// 写入上传信息
        /// </summary>
        /// <param name="batchCode">批次号</param>
        /// <param name="billNo">单据号	</param>
        /// <param name="productionTime">生产日期</param>
        /// <param name="packingTime">组包时间</param>
        /// <param name="plantCode">工厂编码</param>
        /// <param name="productBasicCode">	产品规格编码 </param>
        /// <param name="productBasicId">产品规格id</param>
        /// <param name="productCode">产品编码</param>
        /// <param name="productId">产品id</param>
        /// <param name="workshopCode">车间编码	</param>
        /// <param name="ls">上传列表</param>
        /// <param name="parentcode"></param>
        public void AddUploadStr (string batchCode, string billNo, string productionTime, string packingTime, string plantCode, string productBasicCode, long productBasicId,string productCode,long productId,string workshopCode,List<ch_Collect> ls,string parentcode)
        {
            ch_Uploadstr upstr = new ch_Uploadstr();
            upstr.creatime = DateTime.Now.ToString("yyyy-MM-dd");
            upstr.fcode = parentcode;
            string children = "";
            string str = "{ \"batchCode\": \"" + batchCode + "\",\"billNo\": \"" + billNo + "\",\"createBy\": \"\", \"createTime\": \"\", \"expiryDate\": \"\",\"packingTime\": \"" + packingTime + "\",\"plantCode\": \"" + plantCode + "\",\"productBasicCode\": \"" + productBasicCode + "\"," +
  "\"productBasicId\":" + productBasicId + ",\"productCode\": \"" + productCode + "\",\"productId\":" + productId + ",\"productionTime\": \"" + productionTime + "\",\"relationUploadReqs\": [{\"children\":[";
            foreach (ch_Collect col in ls)
            {
               children += "{ \"children\": [], \"qrCode\": \"" + col.code + "\"},";
              //  children += "{\"qrCode\": \"" + col.code + "\"},";
            }
            children = children.Substring(0, children.Length - 1) + "  ], \"qrCode\": \""+parentcode+"\" } ],";
            str = str + children;
            str += "\"remark\": \"\", \"suitStatus\": 0, \"updateBy\": \"\", \"updateTime\": \"\", \"workshopCode\": \""+ workshopCode + "\"}";

            upstr.uploadstr = str;
            ch_Collect.AddUploadStr(upstr);
        }

        public bool FinshUpLoadinfo(ch_Uploadstr info)
        {
            return ch_Collect.deluploadmodifystatus(info);
        }
    }
}
