using Dll.Base;
using Entity.entity;
using System;
using System.Data;

namespace Dll.Sub
{
    public class RelevanceCodeDal : BaseDal<RelevanceCode>
    {

        public int AddCode(string code, string baseCode)
        {
            string sql = " Insert into " + GenerateSql.GetSqlTableName("RelevanceCode") + " (code, base_code) values ( '" + code + "', '" + baseCode + "')";
            return SqlHelper.MyExecuteNonQuery(GenerateSql.connectionString, CommandType.Text, sql);
        }

        public int GetCount()
        {
            string sql = "select count(code) from " + GenerateSql.GetSqlTableName("RelevanceCode");

            return Convert.ToInt32(SqlHelper.ExecuteScalar(GenerateSql.connectionString, sql));
        }

        public int ClearTable()
        {
            string sql = "delete from " + GenerateSql.GetSqlTableName("RelevanceCode");
            return SqlHelper.MyExecuteNonQuery(GenerateSql.connectionString, CommandType.Text, sql);
        }
    }

}
