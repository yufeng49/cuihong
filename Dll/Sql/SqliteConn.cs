using Common.Sqlite;
using System.Data;

namespace Dll.Sql
{
    public class SqliteConn : BaseSql
    {
        /// <summary>
        /// 单列模式
        /// </summary>
        public static SqliteConn mysqlConn;
        public static SqliteConn CreateInstance()
        {
            if (mysqlConn == null)
                mysqlConn = new SqliteConn();
            return mysqlConn;
        }

        public override int MyExecuteNonQuery(string connectionString, CommandType commandType, string commandText)
        {
            return SqliteHelper.MyExecuteNonQuery(connectionString, commandType, commandText);
        }

        public override DataSet ExecuteDataSet(string connectionString, string commandText)
        {
            return SqliteHelper.ExecuteDataSet(connectionString, commandText);
        }

        public override object ExecuteScalar(string connectionString, string commandText)
        {
            return SqliteHelper.ExecuteScalar(connectionString, commandText);
        }

        public override bool OperateExecuteScalar(string connectionString, CommandType commandType, string code)
        {
            throw new System.NotImplementedException("sqlite 不支持");
        }
    }
}
