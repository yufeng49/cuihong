using BLL.Base;
using Common.Utils;
using Dll.Sub;
using Entity.entity;
using System.Collections.Generic;

namespace BLL.Sub
{
    public class ProductionRecordBll : BaseServer<ProductionRecord>
    {
        private ProductionRecordDal productionRecordDalDal;
        public ProductionRecordBll()
        {
            baseDal = new ProductionRecordDal();
            productionRecordDalDal = baseDal as ProductionRecordDal;
        }

        public int SelectByOrder(string number, string policyLp)
        {
            return productionRecordDalDal.SelectByOrder(number, policyLp);

        }

        public int DelNowTimeData(string date)
        {
            return productionRecordDalDal.DelNowTimeData(date);

        }

        public List<ProductionRecord> GetDataByDate(string beforeDate, string laterDate)
        {
            return TableHelper.DataSetToList<ProductionRecord>(productionRecordDalDal.GetDataByDate(beforeDate, laterDate));

        }

    }
}
