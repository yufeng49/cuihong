﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace Common.Sqlite
{
    /// <summary>
    /// SQLite 助手(密封类不能被继承)
    /// </summary>
    public sealed class SqliteHelper20201229
    {
        private static string connectionString = string.Empty;

        /// <summary>
        /// 根据数据源、密码、版本号设置连接字符串。
        /// </summary>
        /// <param name="datasource">数据源。</param>
        /// <param name="password">密码。</param>
        /// <param name="version">版本号（缺省为3）。</param>
        public static void SetConnectionString(string datasource, string password, string version)
        {
            connectionString = string.Format("Data Source={0};Version={1};password={2}",
               datasource, version, password);
        }
        /// <summary>
        /// 创建一个数据库文件。如果存在同名数据库文件，则会覆盖。
        /// </summary>
        /// <param name="dbName">数据库文件名。为null或空串时不创建。</param>
        public static void CreateDB(string dbName)
        {
            if (!string.IsNullOrEmpty(dbName))
            {
                try
                {
                    SQLiteConnection.CreateFile(dbName);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        /// <summary> 
		/// 对SQLite数据库执行增删改操作，返回受影响的行数。 
		/// </summary> 
		/// <param name="sql">要执行的增删改的SQL语句。</param> 
		/// <param name="parameters">执行增删改语句所需要的参数，参数必须以它们在SQL语句中的顺序为准。</param> 
		/// <returns></returns> 
		/// <exception cref="Exception"></exception>
        public int ExecuteNonQuery(string sql, params SQLiteParameter[] parameters)
        {
            int affectedRows = 0;
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                using (SQLiteCommand comm = new SQLiteCommand(connection))
                {
                    try
                    {
                        connection.Open();
                        comm.CommandText = sql;
                        if (parameters.Length != 0)
                        {
                            comm.Parameters.AddRange(parameters);
                        }
                        affectedRows = comm.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            return affectedRows;
        }

        /// <summary>
		/// 批量处理数据操作语句。
		/// </summary>
		/// <param name="list">SQL语句集合。</param>
		/// <exception cref="Exception"></exception>
        public void ExecuteNonQueryBatch(List<KeyValuePair<string, SQLiteParameter[]>> list)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                }
                catch (Exception)
                {
                    throw;
                }

                using (SQLiteTransaction tran = conn.BeginTransaction())
                {
                    using (SQLiteCommand cmd = new SQLiteCommand(conn))
                    {
                        try
                        {
                            foreach (var item in list)
                            {
                                cmd.CommandText = item.Key;
                                if (item.Value != null)
                                {
                                    cmd.Parameters.AddRange(item.Value);
                                }
                                cmd.ExecuteNonQuery();
                            }
                            tran.Commit();
                        }
                        catch (Exception) { tran.Rollback(); throw; }
                    }
                }
            }
        }


        public object ExecuteScalar(string sql, params SQLiteParameter[] parameters)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                using (SQLiteCommand comm = new SQLiteCommand(conn))
                {
                    try
                    {
                        conn.Open();
                        comm.CommandText = sql;
                        if (parameters.Length != 0)
                        {
                            comm.Parameters.AddRange(parameters);
                        }
                        return comm.ExecuteScalar();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

        }

        /// <summary> 
		/// 执行一个查询语句，返回一个包含查询结果的DataTable。 
		/// </summary> 
		/// <param name="sql">要执行的查询语句。</param> 
		/// <param name="parameters">执行SQL查询语句所需要的参数，参数必须以它们在SQL语句中的顺序为准。</param> 
		/// <returns></returns> 
		/// <exception cref="Exception"></exception>
        public DataTable ExecuteQuery(string sql, params SQLiteParameter[] parameters)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                using (SQLiteCommand comm = new SQLiteCommand(sql, conn))
                {
                    try
                    {
                        if (parameters.Length != 0)
                        {
                            comm.Parameters.AddRange(parameters);
                        }
                        SQLiteDataAdapter adapter = new SQLiteDataAdapter(comm);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                }
            }
        }


        /// <summary> 
		/// 查询数据库中的所有数据类型信息。
		/// </summary> 
		/// <returns></returns> 
		/// <exception cref="Exception"></exception>
		public DataTable GetSchema()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    return connection.GetSchema("TABLES");
                }
                catch (Exception) { throw; }
            }
        }

    }
}
