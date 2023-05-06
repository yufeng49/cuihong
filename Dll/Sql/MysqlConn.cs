using Common.Mysql;
using System.Data;

namespace Dll.Sql
{
    public class MysqlConn : BaseSql
    {
        /// <summary>
        /// 单列模式
        /// </summary>
        public static MysqlConn mysqlConn;
        public static MysqlConn CreateInstance()
        {
            if (mysqlConn == null)
                mysqlConn = new MysqlConn();
            return mysqlConn;
        }
        public override int MyExecuteNonQuery(string connectionString, CommandType commandType, string commandText)
        {
            return MysqlHepler.MyExecuteNonQuery(connectionString, commandType, commandText);
        }

        public override DataSet ExecuteDataSet(string connectionString, string commandText)
        {
            return MysqlHepler.ExecuteDataSet(connectionString, CommandType.Text, commandText);
        }

        public override object ExecuteScalar(string connectionString, string commandText)
        {
            return MysqlHepler.ExecuteScalar(connectionString, CommandType.Text, commandText);
        }

        public override bool OperateExecuteScalar(string connectionString, CommandType commandType, string code)
        {
            return MysqlHepler.OperateExecuteScalar(connectionString, CommandType.Text, code);
        }
    }
}
