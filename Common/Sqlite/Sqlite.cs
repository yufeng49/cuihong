using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Common.Sqlite
{
    public class Sqlite
    {
        SQLiteConnection m_dbConnection = null;
        public Sqlite()
        {
            //  createNewDatabase();
            connectToDatabase();
            createTable();
        }

        //创建一个空的数据库
        void createNewDatabase()
        {
            SQLiteConnection.CreateFile("MyDatabase.db");
        }

        //创建一个连接到指定数据库
        void connectToDatabase()
        {
            m_dbConnection = new SQLiteConnection("Data Source=MyDatabase.db;Version=3;");
            m_dbConnection.Open();
        }

        //在指定数据库中创建一个table
        void createTable()
        {
            SQLiteCommand command = new SQLiteCommand(m_dbConnection);
            command.CommandText = "select count(*) from sqlite_master where type = 'table' and name = 'eliminate'";//and name = 'bagCodeTb' and name = 'boxCodeTb'";            
            if (Convert.ToInt32(command.ExecuteScalar()) == 0)
            {
                command.CommandText = "create table eliminate (name varchar(10), yanshi varchar(10),kaiguan varchar(10),bihe varchar(10),youxiao int, instructions varchar(10), desc varchar(100))";
                command.ExecuteNonQuery();
                fillTable();

                command.CommandText = "create table bagCodeTb (boxCode varchar(20))";
                command.ExecuteNonQuery();

                command.CommandText = "create table uploadInfo (info varchar(1000), remark varchar(20), time varchar(20))";
                command.ExecuteNonQuery();

                command.CommandText = "create table collection (product varchar(100),productID varchar(10), bag varchar(10),box varchar(10),duo varchar(10), relation varchar(10), batch varchar(100), team varchar(10), sureError varchar(10))";
                command.ExecuteNonQuery();
                AddCollectionInfo();

                command.CommandText = "create table betelNutInfo (id varchar(50), name varchar(50), count varchar(10),proportion varchar(50),line varchar(10), orderId varchar(50), orderNumber varchar(50), customer varchar(100), productOrderId varchar(100))";
                command.ExecuteNonQuery();

                command.CommandText = "create table newHopeSet (productName varchar(100),productID varchar(10),ratio varchar(10))";
                command.ExecuteNonQuery();
                AddCollectionInfo();

                command.CommandText = "create table newHopeCode1 (code varchar(50))";
                command.ExecuteNonQuery();
                AddCollectionInfo();

                command.Dispose();
            }  //没有表 添加             

        }

        //添加数据
        void fillTable()
        {
            string sql = "insert into eliminate (name, yanshi,kaiguan,bihe,youxiao,instructions,desc) values ('X1', '50', '1', '50', 1,000,'')";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            sql = "insert into eliminate (name, yanshi,kaiguan,bihe,youxiao,instructions,desc) values ('X2', '50', '2', '50', 1,000,'')";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            sql = "insert into eliminate (name, yanshi,kaiguan,bihe,youxiao,instructions,desc) values ('X3', '50', '3', '50', 1,000,'')";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            sql = "insert into eliminate (name, yanshi,kaiguan,bihe,youxiao,instructions,desc) values ('X4', '50', '4', '50', 1,000,'')";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
            command.Dispose();
        }

        void AddCollectionInfo()//string product, string id, string bag, string box, string relation, string batch, string team, string sureError)
        {
            SQLiteCommand command = new SQLiteCommand(m_dbConnection);
            //string sql = "insert into collection (product, productID, bag, box, relation, batch, team, sureError) values ('" + product + "', '" + id + "', '" + bag + "','" + box + "','" + relation + "','" + batch + "','" + team + "','" + sureError + "')";
            string sql = "insert into collection (product, productID, bag, box,duo, relation, batch, team, sureError) values ('老火锅专用红油', '75', '10','5','1','1','','1','0')";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
        }

        public void AddBetelNutInfo(string Id, string name, string count, string proportion, string line, string orderId, string orderNumber, string customer, string productOrderId)
        {
            SQLiteCommand command = new SQLiteCommand(m_dbConnection);
            string sql = "insert into betelNutInfo (Id,  name,  count,  proportion,  line, orderId, orderNumber,  customer, productOrderId) values ('" + Id + "','" + name + "','" + count + "','" + proportion + "','" + line + "', '" + orderId + "', '" + orderNumber + "',  '" + customer + "', '" + productOrderId + "')";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
        }

        public int AddNewHopeCode(string code)
        {
            SQLiteCommand command = new SQLiteCommand(m_dbConnection);
            string sql = "insert into newHopeCode1 (code) values ('" + code + "')";
            command = new SQLiteCommand(sql, m_dbConnection);
            return command.ExecuteNonQuery();
        }

        public string SelectNewHopeCodeBy(string code)
        {
            string str = "";
            string sql = "select code from newHopeCode1 where code = '" + code + "'";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
                str = reader["code"].ToString();
            reader.Close();
            command.Dispose();
            return str;
        }

        public int DelNewHopeCodeBy(string code)
        {
            SQLiteCommand command = new SQLiteCommand(m_dbConnection);
            string sql = "delete from newHopeCode1 where code ='" + code + "'";
            command = new SQLiteCommand(sql, m_dbConnection);
            return command.ExecuteNonQuery();
        }

        public void AddNewHopeSet(string id, string name, string ratio)
        {
            SQLiteCommand command = new SQLiteCommand(m_dbConnection);
            string sql = "insert into newHopeSet (productName ,productID ,ratio ) values ('" + name + "', '" + id + "', '" + ratio + "')";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
        }

        public void AddEliminateData(string parm1, string parm2, string parm3, string parm4, int parm5, string pram6, string pram7)
        {

            string sql = "insert into eliminate (name, yanshi,kaiguan,bihe,youxiao,instructions,desc) values ('" + parm1 + "', '" + parm2 + "', '" + parm3 + "', '" + parm4 + "', '" + parm5 + "','" + pram6 + "','" + pram7 + "')";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
            command.Dispose();
        }

        public void AddBagCodeTb(string code)
        {
            SQLiteCommand command = new SQLiteCommand(m_dbConnection);
            //string sql = "insert into collection (product, productID, bag, box, relation, batch, team, sureError) values ('" + product + "', '" + id + "', '" + bag + "','" + box + "','" + relation + "','" + batch + "','" + team + "','" + sureError + "')";
            string sql = "insert into bagCodeTb (boxCode) values ('" + code + "')";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
        }

        public string GetBetelNutInfo()
        {
            string str = "";
            string sql = "select * from betelNutInfo";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
                str = reader["Id"].ToString() + "-" + reader["name"].ToString() + "-" + reader["count"].ToString() + "-" + reader["proportion"].ToString() + "-" + reader["line"].ToString() + "-" + reader["orderId"].ToString() + "-" + reader["orderNumber"].ToString() + "-" + reader["customer"].ToString() + "-" + reader["productOrderId"].ToString();
            reader.Close();
            command.Dispose();
            return str;
        }

        public List<string> GetBagCodeTb()
        {
            List<string> listStr = new List<string>();
            string sql = "select boxCode from bagCodeTb";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
                listStr.Add(reader["boxCode"].ToString());
            reader.Close();
            command.Dispose();
            return listStr;

        }

        public void DelBagCodeTb()
        {
            SQLiteCommand command = new SQLiteCommand(m_dbConnection);
            //string sql = "insert into collection (product, productID, bag, box, relation, batch, team, sureError) values ('" + product + "', '" + id + "', '" + bag + "','" + box + "','" + relation + "','" + batch + "','" + team + "','" + sureError + "')";
            string sql = "delete from bagCodeTb";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
        }

        public void DelBetelNutInfo()
        {
            SQLiteCommand command = new SQLiteCommand(m_dbConnection);
            string sql = "delete from betelNutInfo";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
        }

        public bool AddUploadInfo(string info, string remark, string time)
        {
            try
            {
                SQLiteCommand command = new SQLiteCommand(m_dbConnection);
                string sql = "insert into uploadInfo (info, remark,time) values ('" + info + "', '" + remark + "', '" + time + "')";
                command = new SQLiteCommand(sql, m_dbConnection);
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int GetUploadInfoCount()
        {
            try
            {
                string sql = "select count(*) from uploadInfo";
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                //SQLiteDataReader reader = command.ExecuteReader();
                int count = Convert.ToInt32(command.ExecuteScalar());
                command.Dispose();
                return count;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public string GetCollection()
        {
            string listStr = "";
            string sql = "select  * from collection ";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
                listStr = reader["product"] + "_" + reader["productID"] + "_" + reader["bag"] + "_" + reader["box"] + "_" + reader["duo"] + "_" + reader["relation"] + "_" + reader["batch"] + "_" + reader["team"] + "_" + reader["sureError"];
            reader.Close();
            command.Dispose();
            return listStr;
        }

        public string GetNewHopeSet()
        {
            string listStr = "";
            string sql = "select  * from newHopeSet ";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
                listStr = reader["productName"] + "_" + reader["productID"] + "_" + reader["ratio"];
            reader.Close();
            command.Dispose();
            return listStr;
        }

        public bool AddBoxCodeTb(string boxCode, string bagCode, string stackCode)
        {
            try
            {
                string sql = "insert into boxCodeTb (boxCode, bagCode, stackCode) values ('" + bagCode + "', '" + boxCode + "', '" + stackCode + "')";
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                command.ExecuteNonQuery();
                command.Dispose();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //得到数据
        public List<string> GetList()
        {
            List<string> listStr = new List<string>();
            string sql = "select * from eliminate";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
                listStr.Add(string.Format(reader["name"] + ":" + reader["yanshi"] + ":" + reader["kaiguan"] + ":" + reader["bihe"] + ":" + reader["youxiao"] + ":" + reader["instructions"] + ":" + reader["desc"]));
            command.Dispose();
            reader.Close();
            return listStr;
        }

        public List<string> GetUpLoadList()
        {
            List<string> listStr = new List<string>();
            string sql = "select * from uploadInfo ";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
                listStr.Add(reader["info"].ToString());
            command.Dispose();
            reader.Close();
            return listStr;
        }

        public bool DelUpLoad()
        {
            try
            {
                List<string> listStr = new List<string>();
                string sql = "delete from uploadInfo";
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                SQLiteDataReader reader = command.ExecuteReader();
                command.Dispose();
                reader.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            };
        }

        public void DelNewHopeSet()
        {
            SQLiteCommand command = new SQLiteCommand(m_dbConnection);
            //string sql = "insert into collection (product, productID, bag, box, relation, batch, team, sureError) values ('" + product + "', '" + id + "', '" + bag + "','" + box + "','" + relation + "','" + batch + "','" + team + "','" + sureError + "')";
            string sql = "delete from newHopeSet";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
        }

        public string GetOneData(string name)
        {
            string sql = "select instructions,yanshi,bihe from eliminate where name ='" + name + "'";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            string str = "";
            while (reader.Read())
                str += reader["instructions"] + "-" + reader["yanshi"] + "-" + reader["bihe"] + ",";
            command.Dispose();
            reader.Close();
            if (str.Length < 1)
            {
                return "";
            }
            return str.Substring(0, str.Length - 1);
        }

        public int UpdateData(string parm1, string parm2, string parm3, string parm4, int parm5, string pram6, string pram7)
        {
            string sql = "update eliminate set yanshi = '" + parm2 + "',kaiguan='" + parm3 + "',bihe='" + parm4 + "',youxiao='" + parm5 + "',instructions='" + pram6 + "',desc='" + pram7 + "' where name ='" + parm1 + "'";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            int row = command.ExecuteNonQuery();
            command.Dispose();
            return row;
        }

        public int UpdateCollection(string product, string id, string bag, string box, string duo, string relation, string batch, string team, string sureError)
        {
            string sql = "update collection set product = '" + product + "',productID='" + id + "',bag='" + bag + "',box='" + box + "',duo='" + duo + "',relation='" + relation + "',batch= '" + batch + "',team='" + team + "',sureError='" + sureError + "'";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            int row = command.ExecuteNonQuery();
            command.Dispose();
            return row;
        }

        public void DelEliminate()
        {
            SQLiteCommand command = new SQLiteCommand(m_dbConnection);
            //string sql = "insert into collection (product, productID, bag, box, relation, batch, team, sureError) values ('" + product + "', '" + id + "', '" + bag + "','" + box + "','" + relation + "','" + batch + "','" + team + "','" + sureError + "')";
            string sql = "delete from eliminate";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
        }

        public void Close()
        {
            m_dbConnection.Close();
        }

    }

}
