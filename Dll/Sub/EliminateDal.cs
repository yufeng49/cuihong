using Common.Utils;
using Dll.Base;
using Entity.entity;
using System.Collections.Generic;

namespace Dll.Sub
{
    public class EliminateDal : BaseDal<EliminateModel>
    {
        string sqlType = ConfigHelper.GetValue("SqlType", "0");
        public List<EliminateModel> SelectBySearch(string condition)
        {
            string sql = "select * from t_eliminate_model where command_name='" + condition + "'";
            return TableHelper.DataSetToList<EliminateModel>(SqlHelper.ExecuteDataSet(GenerateSql.connectionString, sql));

        }
    }
}
