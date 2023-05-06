
using Common.Utils;
using Dll.Sql;
using System;
using System.Collections.Generic;
using System.Data;

namespace Dll.Base
{

    public class BaseDal<T> //: IBaseDal<T>
    {
        protected static BaseSql SqlHelper;
        string sqlType = ConfigHelper.GetValue("SqlType", "0");
        public BaseDal()
        {
            if (sqlType == "Sqlite")
            {
                SqlHelper = SqliteConn.CreateInstance();
                //初始化加载
                TableIsExistSqlite();
            }
            else
            {
                SqlHelper = MysqlConn.CreateInstance();
                TableIsExistMysql();
            }
        }

        public int Add(T t)
        {
            string sql = GenerateSql.GenerateInsert<T>(t);
            return SqlHelper.MyExecuteNonQuery(GenerateSql.connectionString, CommandType.Text, sql);
        }

        public int Delete(int id)
        {
            string sql = GenerateSql.GenerateDelete<T>(id);
            return SqlHelper.MyExecuteNonQuery(GenerateSql.connectionString, CommandType.Text, sql);
        }

        public int Delete(Dictionary<string, string> dic)
        {
            string sql = GenerateSql.GenerateDeleteByWhere<T>(dic);
            return SqlHelper.MyExecuteNonQuery(GenerateSql.connectionString, CommandType.Text, sql);
        }

        public T Update(T t)
        {
            string sql = GenerateSql.GenerateUpdate<T>(t);
            SqlHelper.MyExecuteNonQuery(GenerateSql.connectionString, CommandType.Text, sql);
            return t;
        }

        public virtual DataSet Select(Dictionary<string, string> dic = null)
        {
            string sql = GenerateSql.SelectBySearch<T>(dic);
            //object obj = SqliteHelper.GetDataTable(GenerateSql.connectionString, sql);
            // var ds = SqliteHelper.ExecuteDataSet(GenerateSql.connectionString, sql);
            return SqlHelper.ExecuteDataSet(GenerateSql.connectionString, sql);
        }

        public virtual DataSet Select(int id)
        {
            Type t = typeof(T);
            string className = t.Name;
            string sql = "Select * from " + GenerateSql.GetSqlTableName(className) + " where id = " + id;
            return SqlHelper.ExecuteDataSet(GenerateSql.connectionString, sql);
        }

        public int GetSum()
        {
            Type t = typeof(T);
            string className = GenerateSql.GetSqlTableName(t.Name);
            string sql = "select count(*) from " + className;
            return Convert.ToInt32(SqlHelper.ExecuteScalar(GenerateSql.connectionString, sql));
        }
        /// <summary>
        /// 判断表是否存在 不存在添加
        /// </summary>
        /// <returns></returns>
        public void TableIsExistSqlite()
        {
            Type t = typeof(T);
            string className = GenerateSql.GetSqlTableName(t.Name);
            string sql = "select count(*) from sqlite_master where type = 'table' and name = '" + className + "'";
            var obj = SqlHelper.ExecuteScalar(GenerateSql.connectionString, sql);
            if (Convert.ToInt32(obj) == 0)
            { //添加
                string sqlls = GenerateSql.GenerateAddTable<T>();
                SqlHelper.MyExecuteNonQuery(GenerateSql.connectionString, CommandType.Text, sqlls);
            }
        }

        /// <summary>
        /// 判断表是否存在 不存在添加
        /// </summary>
        /// <returns></returns>
        public void TableIsExistMysql()
        {
            Type t = typeof(T);
            string className = GenerateSql.GetSqlTableName(t.Name);
            string sql = "select count(*) from information_schema.TABLES t where t.TABLE_SCHEMA ='mydatabase' and t.TABLE_NAME ='" + className + "'";
            var obj = SqlHelper.ExecuteScalar(GenerateSql.connectionString, sql);
            if (Convert.ToInt32(obj) == 0)
            { //添加
                string sqlls = GenerateSql.GenerateAddTableMysql<T>();
                SqlHelper.MyExecuteNonQuery(GenerateSql.connectionString, CommandType.Text, sqlls);
            }
        }
    }
}
