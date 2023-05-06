using Dll.Base;
using Entity.entity;
using System;
using System.Data;

namespace Dll.Sub
{
    public class ProductionRecordDal : BaseDal<ProductionRecord>
    {
        public int SelectByOrder(string number, string policyLp)
        {
            try
            {
                string sql = "select count(order_number) from " + GenerateSql.GetSqlTableName("ProductionRecord") + " where order_number = '" + number + "' and policy = '" + policyLp + "'";

                return Convert.ToInt32(SqlHelper.ExecuteScalar(GenerateSql.connectionString, sql));
            }
            catch (Exception ex)
            {

                return 0;
            }
        }

        public DataSet GetDataByDate(string beforeDate, string laterDate)
        {
            string sql = "SELECT * FROM " + GenerateSql.GetSqlTableName("ProductionRecord") + " where " + GenerateSql.GetSqlFiledName("CreateTime") + " Between '" + beforeDate + "' AND '" + laterDate + "'";
            return SqlHelper.ExecuteDataSet(GenerateSql.connectionString, sql);
        }


        public int DelNowTimeData(string date)
        {
            try
            {
                string sql = "delete  from " + GenerateSql.GetSqlTableName("ProductionRecord") + " where create_time < '" + date + "'";

                return Convert.ToInt32(SqlHelper.ExecuteScalar(GenerateSql.connectionString, sql));
            }
            catch (Exception ex)
            {

                return 0;
            }
        }
    }
}
