using Dll.Base;
using Entity.entity;
using System;
using System.Collections.Generic;

namespace Dll.Sub
{
    public class PrintConfigDal : BaseDal<PrintConfig>
    {

    }


    public class BaseConfigDal : BaseDal<BaseConfig>
    { 
    
    }

    public class ProdInfoDal : BaseDal<ProdInfo>
    {
     
        public int CheckAndAddData(List<KangmeiEntity> data)
        {
            int ret = 0;
            foreach (KangmeiEntity ke in data)
            {
                string sql = "select count(prod_id) from  t_prod_info where prod_id ='" +ke.Id +"'";
               int cc = Convert.ToInt32(SqlHelper.ExecuteScalar(GenerateSql.connectionString, sql));
                if (cc > 0)
                {
                    //   ke.resProdCodeList
                    string verify = "";
                    string pkg = "";
                    foreach (KangmeiSpec kms in ke.resProdCodeList)
                    {
                        //只取小码的验证规则
                        if (kms.codeLevel.Equals("1"))
                        {
                            verify = kms.codeVersion + kms.value;
                            pkg = kms.pkgRatio;
                            break;
                        }
                    }
                    string cmd = "update t_prod_info set product_name ='" + ke.productName + "'," +
                        " sub_type ='" + ke.subType + "'," +
                        "spec_desc ='" + ke.spec + "'," +
                        "package_spec='" + ke.packageSpec + "'," +
                        "verfy_code='" + verify + "'," +
                        "pkg_ratio='" + pkg + "' where prod_id = "+ ke.Id;
                    SqlHelper.MyExecuteNonQuery(GenerateSql.connectionString, System.Data.CommandType.Text, cmd);
                }
                else
                {
                    //  insert
                    string verify = "";
                    string pkg = "";
                    foreach (KangmeiSpec kms in ke.resProdCodeList)
                    {
                        //只取小码的验证规则
                        if (kms.codeLevel.Equals("1"))
                        {
                            verify = kms.codeVersion + kms.value;
                            pkg = kms.pkgRatio;
                            break;
                        }
                    }
                    string cmd = "INSERT INTO t_prod_info(\"product_name\", \"sub_type\", \"sub_typeno\",\"spec_desc\", \"package_spec\", \"verfy_code\", \"pkg_ratio\", \"collect_count\", \"prod_id\") VALUES (" +
                       " '" + ke.productName + "','" + ke.subType + "','" +ke.subTypeNo+"','"+ ke.spec + "','"+ke.packageSpec+"','" + verify + "','" + pkg + "',5,'" + ke.Id + "')";
                    SqlHelper.MyExecuteNonQuery(GenerateSql.connectionString, System.Data.CommandType.Text, cmd);
                    ret++;
                }
            }

            return ret;
        }

        public int UpdateByProdid(ProdInfo pi)
        {
            string sql = "update t_prod_info set collect_count='" + pi.CollectCount + "' where prod_id='" + pi.ProdId + "'";

            return SqlHelper.MyExecuteNonQuery(GenerateSql.connectionString, System.Data.CommandType.Text, sql);
        }
    }
}
