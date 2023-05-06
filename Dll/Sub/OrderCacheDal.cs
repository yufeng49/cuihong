using Common.Utils;
using Dll.Base;
using Entity.entity;
using System;
using System.Collections.Generic;
using System.Data;

namespace Dll.Sub
{
    public class EshengEntityDal : BaseDal<EshengEntity>
    {
        string LogPath = "D:\\Debug\\Err";
        string sqlType = ConfigHelper.GetValue("SqlType", "0");
        //public EshengEntityDal()
        //{
        //    if (sqlType == "Sqlite")
        //    {
        //        SqlHelper = SqliteConn.CreateInstance();
        //        //初始化加载
        //        TableIsExistSqlite();
        //    }
        //    else
        //    {
        //        SqlHelper = MysqlConn.CreateInstance();
        //        TableIsExistMysql();
        //    }
        //}

        /// <summary>
        /// 检测数据是否存在
        /// </summary>
        /// <param name="randcode"></param>
        /// <returns></returns>
        public bool CheckExist(string condition, string values)
        {
            bool flag = false;
            string sql = "select count(id) from t_esheng_entity where " + condition + "='" + values + "'";
            int ret = Convert.ToInt32(SqlHelper.ExecuteScalar(GenerateSql.connectionString, sql));
            flag = ret > 0 ? true : false;
            return flag;
        }

        public void DeleteALl()
        {
            string sql = "delete from t_esheng_entity";
            SqlHelper.ExecuteScalar(GenerateSql.connectionString, sql);
        }

        public List<EshengEntity> SearchDataByCondition(string condition)
        {
            condition = condition.Length > 0 ? "where " + condition : "";
            string sql = "select id,random_code,material_name,material_no,weight_type,printtime,collecttime,inwarehouse,qr_code,weight,inwarehouse from t_esheng_entity   " + condition;
            return TableHelper.DataSetToList<EshengEntity>(SqlHelper.ExecuteDataSet(GenerateSql.connectionString, sql));

        }

        public bool UpdateEntity(EshengEntity entity)
        {
            bool flag = false;

            string sql = "update t_esheng_entity set collecttime ='" + entity.Collecttime + "',inwarehouse=" + entity.Status + "  where id=" + entity.Id;
            int ret = SqlHelper.MyExecuteNonQuery(GenerateSql.connectionString, System.Data.CommandType.Text, sql);
            flag = ret > 0 ? true : false;
            return flag;
        }
        public bool DeleteEntity(string qrcode)
        {
            bool flag = false;
            string sql = "delete  from   t_esheng_entity where qr_code ='" + qrcode + "'";
            int ret = SqlHelper.MyExecuteNonQuery(GenerateSql.connectionString, System.Data.CommandType.Text, sql);
            flag = ret > 0 ? true : false;
            return flag;
        }

        public bool InsertData(EshengEntity ee)
        {
            bool flag = false;
            string searchsql = "select count(id) from t_esheng_entity where id = '" + ee.Id + "'";

            int count = Convert.ToInt32(SqlHelper.ExecuteScalar(GenerateSql.connectionString, searchsql));
            if (count > 0)
            {
                //已有数据 不做插入
                return true;
            }
            else
            {
                string sql = "INSERT INTO t_esheng_entity( \"create_dept\", \"create_time\", \"create_times\", \"create_user\", \"hdno\", \"Is_deleted\", \"leave_factory_no\", \"material_id\", \"material_name\", \"material_no\",\"qr_code\", \"qr_codeflow\", \"random_code\", \"status\", \"update_time\", \"update_user\", \"weight\", \"weight_type\", \"batch\", \"printtime\", \"collecttime\", \"inwarehouse\",\"id\") VALUES " +
                    "( '" + ee.CreateDept + "', '" + ee.CreateTime + "', '" + ee.CreateTimes + "', '" + ee.CreateUser + "', '" + ee.Hdno + "', '" + ee.IsDeleted + "', '" + ee.LeaveFactoryNo + "', '" + ee.MaterialId + "', '" + ee.MaterialName + "', '" + ee.MaterialNo + "', '" + ee.QrCode + "', '" + ee.QrCodeflow + "', '" + ee.RandomCode + "', '0', '" + ee.UpdateTime + "', '" + ee.UpdateUser + "', " + ee.Weight + ", '" + ee.WeightType + "', '" + ee.Batch + "', '" + ee.Printtime + "', NULL, 1," + ee.Id + ")";

                try
                {
                    int ret = Convert.ToInt32(SqlHelper.MyExecuteNonQuery(GenerateSql.connectionString, System.Data.CommandType.Text, sql));
                    flag = ret > 0 ? true : false;
                }
                catch (Exception ex)
                {

                    LogHelper.WriteLog(LogPath, "插入异常: " + sql + " 异常信息:" + ex.Message);
                }
            }
            return flag;
        }
        public List<EshengEntity> SearchData(string batch)
        {
            string sql = "select id,random_code,material_name,material_no,weight_type,printtime,collecttime,inwarehouse,qr_code,weight,inwarehouse from t_esheng_entity where batch ='" + batch + "' order by id desc";
            return TableHelper.DataSetToList<EshengEntity>(SqlHelper.ExecuteDataSet(GenerateSql.connectionString, sql));
        }
    }

    public class EshengCheckoutDal : BaseDal<EshengCheckout>
    {
        public void DeleteData(string dispatchOrderId)
        {
            string sql = "delete from t_esheng_checkout WHERE dispatch_order_id='" + dispatchOrderId + "'";
            SqlHelper.MyExecuteNonQuery(GenerateSql.connectionString, System.Data.CommandType.Text, sql);
        }

        public bool FinshData(string dispatchOrderId, string Qrcode)
        {
            bool flag = false;
            string sql = "UPDATE t_esheng_checkout SET  oout_status=1 ,qrcode='" + Qrcode + "' WHERE dispatch_order_id='" + dispatchOrderId + "'";
            int ret = Convert.ToInt32(SqlHelper.MyExecuteNonQuery(GenerateSql.connectionString, System.Data.CommandType.Text, sql));
            flag = ret > 0 ? true : false;
            return flag;
        }

        public List<qrcodeinfo> GetDetailData(string order)
        {
            string sql = "select * from t_esheng_checkout_detail where out_num ='" + order + "'";
            List<qrcodeinfo> retls = new List<qrcodeinfo>();
            DataTable dt = SqlHelper.ExecuteDataSet(GenerateSql.connectionString, sql).Tables[0];

            //List<EshengCheckout_detail> ls = TableHelper.DataSetToList<EshengCheckout_detail>(SqlHelper.ExecuteDataSet(GenerateSql.connectionString, sql));


            //foreach (EshengCheckout_detail ed in ls)
            //{

            //    retls.Add(new qrcodeinfo()
            //    {
            //        leaveFactoryNo = ed.leaveFactoryNo,
            //        materialName = ed.materialName,
            //        materialNo = ed.materialNo,
            //        qrCode = ed.qrCode,
            //        weight = ed.weight,
            //        weightType = ed.weight,
            //        status = ed.qrState

            //    });
            //}


            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    retls.Add(new qrcodeinfo()
                    {
                        leaveFactoryNo = dr["leave_factory_no"].ToString(),
                        materialNo = dr["material_no"].ToString(),
                        materialName = dr["material_name"].ToString(),
                        qrCode = dr["qr_code"].ToString(),
                        weight = dr["weight"].ToString(),
                        weightType = dr["weight"].ToString(),
                        status = dr["qr_state"].ToString(),
                    });
                }
            }

            return retls;
        }

        public bool InsertData(EshengCheckout et)
        {
            bool flag = false;
            string searchsql = "select count(id) from t_esheng_checkout where dispatch_order_id = '" + et.DispatchOrderId + "'";
            int count = Convert.ToInt32(SqlHelper.ExecuteScalar(GenerateSql.connectionString, searchsql));
            if (count > 0)
            {
                //已有数据 做更新
                string sql = "UPDATE t_esheng_checkout SET  \"out_num\" = " + et.OutNum + ", \"out_weight\" = '" + et.OutWeight + "', \"total_weight\" = '" + et.TotalWeight + "', \"qrcode\"='' WHERE dispatch_order_id='" + et.DispatchOrderId + "'";
                int ret = Convert.ToInt32(SqlHelper.MyExecuteNonQuery(GenerateSql.connectionString, System.Data.CommandType.Text, sql));
                flag = ret > 0 ? true : false;
                return true;
            }
            else
            {

                string sql = "INSERT INTO t_esheng_checkout (\"car_model\", \"dispatch_order_id\", \"dispatch_order_type\", \"factory_code\", \"factory_name\", \"hd_no\", \"leave_factory_no\", \"leave_num\", \"license_plate_no\", \"material_id\", \"material_name\", \"material_no\", \"out_num\", \"out_weight\", \"total_weight\", \"oout_status\", \"out_time\",  \"id\") VALUES(" +
                 "'" + et.CarModel + "','" + et.DispatchOrderId + "','" + et.DispatchOrderType + "','" + et.FactoryCode + "','" + et.FactoryName + "','" + et.HdNo + "','" + et.LeaveFactoryNo + "','" + et.LeaveNum + "','" + et.LicensePlateNo + "','" + et.MaterialId + "','" + et.MaterialName + "','" + et.MaterialNo + "','" + et.OutNum + "','" + et.OutWeight + "'," + et.TotalWeight + ",'0','" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "'," + et.Id + ")";
                int ret = Convert.ToInt32(SqlHelper.MyExecuteNonQuery(GenerateSql.connectionString, System.Data.CommandType.Text, sql));
                flag = ret > 0 ? true : false;
            }
            return flag;
        }

        public bool Insert_DetailData(string outorder, qrcodeinfo et)
        {
            bool flag = false;
            string sql = "insert into t_esheng_checkout_detail (\"out_num\",\"qr_code\",\"write_date\",\"qr_state\",\"material_name\",\"weight\",\"leave_factory_no\",\"material_no\") VALUES(" +
                "'" + outorder + "','" + et.qrCode + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',1,'" + et.materialName + "','" + et.weight + "','" + et.leaveFactoryNo + "','" + et.materialNo + "')";
            int ret = Convert.ToInt32(SqlHelper.MyExecuteNonQuery(GenerateSql.connectionString, System.Data.CommandType.Text, sql));
            flag = ret > 0 ? true : false;
            return flag;
        }
        public List<EshengCheckout> SearchData(string dispatch_order_id)
        {
            string sql = "select * from t_esheng_checkout where dispatch_order_id='" + dispatch_order_id + "'";
            return TableHelper.DataSetToList<EshengCheckout>(SqlHelper.ExecuteDataSet(GenerateSql.connectionString, sql));
        }

        public List<EshengCheckout> SearchDataByCondition(string condition)
        {
            string sql = "select * from t_esheng_checkout where  " + condition;
            return TableHelper.DataSetToList<EshengCheckout>(SqlHelper.ExecuteDataSet(GenerateSql.connectionString, sql));
        }

        public void UpdateList(List<string> qrls)
        {
            string sql = "update t_esheng_checkout_detail set qr_state=2 where qr_code in(";
            for (int i = 0; i < qrls.Count; i++)
            {
                sql += "'" + qrls[i] + "',";
            }
            sql = sql.Substring(0, sql.Length - 1) + ")";
            Convert.ToInt32(SqlHelper.MyExecuteNonQuery(GenerateSql.connectionString, System.Data.CommandType.Text, sql));
        }
    }


}
