using System.Data;

namespace Dll.Sql
{
    public abstract class BaseSql
    {
        public abstract int MyExecuteNonQuery(string connectionString, CommandType commandType, string commandText);

        public abstract DataSet ExecuteDataSet(string connectionString, string commandText);

        public abstract object ExecuteScalar(string connectionString, string commandText);

        public abstract bool OperateExecuteScalar(string connectionString, CommandType commandType, string code);
    }
}
