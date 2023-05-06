using BLL.Base;
using Dll.Sub;
using Entity.entity;
using System;
using System.Collections.Generic;

namespace BLL.Sub
{
    public class PrintConfigBll : BaseServer<PrintConfig>
    {
        public PrintConfigBll()
        {
            baseDal = new PrintConfigDal();
        }
    }

    public class BaseConfigBll : BaseServer<BaseConfig>
    {
        public BaseConfigBll()
        {
            baseDal = new BaseConfigDal();
        }
        public void DeleteData()
        {

            List<string> ls = new List<string>() {  };
            ls.Add("产品名称");
            ls.Add("比例");
            ls.Add("班组");
            ls.Add("prodid");
            ls.Add("包装日期");
            ls.Add("批号");
            ls.Add("产线");
            ls.Add("订单号");
            ls.Add("生产日期");
            foreach (string str in ls)
            {

                Dictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add("Bckey", str);
                baseDal.Delete(dict);
               
            }
        }
        /// <summary>
        /// 检测是否有对应键，有则更新，没有则插入
        /// </summary>
        /// <param name="bc"></param>
        /// <returns></returns>
        public bool CheckAndAdd(BaseConfig bc)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("Bckey", bc.Bckey);
            baseDal.Delete(dict);
            return baseDal.Add(bc) > 0;
        }

        public void DeleteKey(List<string> keyls)
        {
            foreach (string str in keyls)
            {
                Dictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add("Bckey", str);
                baseDal.Delete(dict);
            }
          
        }

    }
    public class ProdInfoBll : BaseServer<ProdInfo>
    {
        private ProdInfoDal pidal;
        public ProdInfoBll()
        {
            baseDal = new ProdInfoDal();
            pidal = baseDal as ProdInfoDal;
        }

        public int CheckAndAdd(List<KangmeiEntity> data)
        {
          return  pidal.CheckAndAddData(data);
        }
     
        public int UpdateByProdid(ProdInfo pi)
        {
          return   pidal.UpdateByProdid(pi);
        }
    }
}
