using BLL.Base;
using Dll.Sub;
using Entity.entity;
using System.Collections.Generic;

namespace BLL.Sub
{
    public class EshengEntityBll : BaseServer<EshengEntity>
    {
        private EshengEntityDal eedal;
        public EshengEntityBll()
        {

            baseDal = new EshengEntityDal();
            eedal = baseDal as EshengEntityDal;
        }

        public bool InsertData(EshengEntity et)
        {

            return eedal.InsertData(et);
        }

        public List<EshengEntity> SearchData(string batch)
        {
            return eedal.SearchData(batch);
        }

        /// <summary>
        /// 根据id更新采集时间和状态
        /// </summary>
        /// <param name="entity">需要更新的数据</param>
        /// <returns></returns>
        public bool UpdateEntity(EshengEntity entity)
        {
            return eedal.UpdateEntity(entity);
        }
        public bool DeleteEntity(string randomcode)
        {
            return eedal.DeleteEntity(randomcode);
        }
        public List<EshengEntity> SearchDataByCondition(string condition)
        {
            return eedal.SearchDataByCondition(condition);
        }

        public void DeleteAll()
        {
            eedal.DeleteALl();

        }
    }


    public class EshengCheckoutBll : BaseServer<EshengCheckout>
    {
        private EshengCheckoutDal eedal;
        public EshengCheckoutBll()
        {

            baseDal = new EshengCheckoutDal();
            eedal = baseDal as EshengCheckoutDal;
        }
        public bool InsertData(EshengCheckout et)
        {
            return eedal.InsertData(et);
        }
        public bool InsertData(string order, qrcodeinfo et)
        {
            return eedal.Insert_DetailData(order, et);
        }
        public List<qrcodeinfo> GetDetailData(string order)
        {
            return eedal.GetDetailData(order);
        }

        public bool FinshData(string prderid, string qr)
        {
            return eedal.FinshData(prderid, qr);
        }

        public List<EshengCheckout> SearchDataByCondition(string condition)
        {
            return eedal.SearchDataByCondition(condition);
        }

        public void DeleteData(string order)
        {
            eedal.DeleteData(order);
        }

        public void UpdateDetailStatus(List<string> qrls)
        {
            eedal.UpdateList(qrls);
        }
    }
}
