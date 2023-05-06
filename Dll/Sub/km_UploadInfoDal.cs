using Dll.Base;
using Entity.entity;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDbSpider;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dll.Sub
{

    public class km_UploadInfoDal 
    {
        MongoDbSpider.MongoHelper mongo = new MongoDbSpider.MongoHelper(GenerateSql.MongoDBconnectionString, "kmdb", "km_UploadInfo");

        public int AddOrder(km_UploadInfo kmupinfo)
        {
            int ret = 0;
            try
            {
                DbMessage s = mongo.Insert(kmupinfo);
                ret = 1;
            }
            catch
            {
            }
            return ret;
        }
        public km_UploadInfo GetUploadinfo(string v)
        {
            FilterDefinitionBuilder<km_UploadInfo> builderFilter = Builders<km_UploadInfo>.Filter;
            FilterDefinition<km_UploadInfo> filter = builderFilter.And(builderFilter.Eq("UploadOrder", v));

            // FilterDefinition<BsonDocument>(.Eq("UploadOrder", order) &Builders<BsonDocument>.Filter.Eq("upstatus",0)
            List<km_UploadInfo> ls = mongo.QueryMany<km_UploadInfo>(filter, 1);
            return ls[0];
        }

        public long km_UpdateStatus(string orderno)
        {
            FilterDefinitionBuilder<BsonDocument> builder = new FilterDefinitionBuilder<BsonDocument>();
            FilterDefinition<BsonDocument> filter = builder.And(builder.Eq("UploadOrder", orderno), builder.Eq("upstatus", 0));


            var updated = Builders<BsonDocument>.Update.Set("upstatus", 1);
            var db = mongo.UpdateMany(filter, updated);
            return db.iFlg;
        }

        public bool CheckOrder(string orderno)
        {
            FilterDefinitionBuilder<km_UploadInfo> builderFilter = Builders<km_UploadInfo>.Filter;
            FilterDefinition<km_UploadInfo> filter = builderFilter.And(builderFilter.Eq("UploadOrder", orderno));

            // FilterDefinition<BsonDocument>(.Eq("UploadOrder", order) &Builders<BsonDocument>.Filter.Eq("upstatus",0)
            List<km_UploadInfo> ls = mongo.QueryMany<km_UploadInfo>(filter, 1);
            if (ls.Count > 0)
            {
                return ls[0].upstatus == 1;
            }
            else
            {
                return true;
            }
        }
    }

    public class km_CollectCodeDal
    {
        MongoDbSpider.MongoHelper mongo = new MongoDbSpider.MongoHelper(GenerateSql.MongoDBconnectionString, "kmdb", "km_CollectCode");

        /// <summary>
        /// 通过小码查询，这一件的所有码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public List<km_CollectCode> Searchlcode2bylocde1(string code)
        {
            FilterDefinitionBuilder<km_CollectCode> builderFilter = Builders<km_CollectCode>.Filter;
            FilterDefinition<km_CollectCode> filter = builderFilter.And(builderFilter.Eq("lcode1", code));
            List<km_CollectCode> retval = new List<km_CollectCode>();
           List<km_CollectCode> ls  = mongo.QueryMany<km_CollectCode>(filter,0);
            if (ls.Count > 0)
            {
                FilterDefinitionBuilder<km_CollectCode> builderFilter1 = Builders<km_CollectCode>.Filter;
                FilterDefinition<km_CollectCode> filter1 = builderFilter1.And(builderFilter.Eq("lcode2", ls[0].lcode2));
                retval = mongo.QueryMany<km_CollectCode>(filter1, 0);
              

            }
            return retval;
        }
        /// <summary>
        /// 根据码查询订单
        /// </summary>
        /// <param name="code">码</param>
        /// <param name="level">lcode1 小码 lcode2 件码 lcode3箱码</param>
        /// <returns></returns>
        public string SearchCodeByOrder(string code,string level)
        {
            FilterDefinitionBuilder<km_CollectCode> builderFilter = Builders<km_CollectCode>.Filter;
            FilterDefinition<km_CollectCode> filter = builderFilter.And(builderFilter.Eq(level, code));
            var retval  = mongo.QueryMany<km_CollectCode>(filter, 0);
            if (retval.Count > 0)
            {
              return  retval[0].UploadOrder;
            }
            else {
                return "";
            }
        }
        public List<km_CollectCode> SearchBaglistBycode(string code)
        {
            FilterDefinitionBuilder<km_CollectCode> builderFilter = Builders<km_CollectCode>.Filter;
            FilterDefinition<km_CollectCode> filter = builderFilter.And(builderFilter.Eq("lcode1", code));

            List< km_CollectCode > ls   = mongo.QueryMany<km_CollectCode>(filter,0);
            if (ls.Count > 0)
            {
                km_CollectCode singledata = ls[0];
                return mongo.QueryMany<km_CollectCode>(Builders<km_CollectCode>.Filter.Eq("lcode3", singledata.lcode3), 0);
            }
            else
            {
                return new List<km_CollectCode>();
            }
        }
        public List<km_CollectCode> SearchBaglistBycode(string code,string level)
        {

            FilterDefinitionBuilder<km_CollectCode> builderFilter = Builders<km_CollectCode>.Filter;
            FilterDefinition<km_CollectCode> filter = builderFilter.And(builderFilter.Eq(level, code));

           return mongo.QueryMany<km_CollectCode>(filter, 0);

        }

        public bool Isexist(string code)
        {
            FilterDefinitionBuilder<km_CollectCode> builderFilter = Builders<km_CollectCode>.Filter;
            FilterDefinition<km_CollectCode> filter = builderFilter.And(builderFilter.Eq("lcode1", code));
            return mongo.QueryMany<km_CollectCode>(filter, 0).Count>0;

        }

        public int DelCollectByCode(string code, string level)
        {
            FilterDefinitionBuilder<BsonDocument> builder = new FilterDefinitionBuilder<BsonDocument>();
            FilterDefinition<BsonDocument> filter = builder.And(builder.Eq(level, code));
        return  Convert.ToInt32(  mongo.DeleteMany(filter, true).iFlg);
        }

        public int CheckLevel(string code)
        {
            int level = 0;
            level = mongo.QueryMany<km_CollectCode>(Builders<km_CollectCode>.Filter.Eq("lcode3", code), 0).Count > 0 ? 3 : 0;
            if (level != 0)
            {
                return level;
            }
            else
            {
                level = mongo.QueryMany<km_CollectCode>(Builders<km_CollectCode>.Filter.Eq("lcode1", code), 0).Count > 0 ? 1 : 0;
                if (level != 0) return level;
                else
                {
                    level = mongo.QueryMany<km_CollectCode>(Builders<km_CollectCode>.Filter.Eq("lcode2", code), 0).Count > 0 ? 2 : 0;
                    return level;
                }
            }
        }

        public bool ReplacementCode(List<string> oldls, List<string> newls, out string msg)
        {
            string val = "";
            msg = "";
            bool flag = false;
            if (oldls.Count != newls.Count)
            {
                msg = "置换数量不匹配";
            }
            else
            {
               
                FilterDefinitionBuilder<BsonDocument> builder = new FilterDefinitionBuilder<BsonDocument>();
                for (int i=0;i<oldls.Count;i++)
                {
                    FilterDefinition<BsonDocument> filter = builder.And(builder.Eq("lcode1", oldls[i]), builder.Ne("upstatus", 2));
                    var updated = Builders<BsonDocument>.Update.Set("lcode1", newls[i]);
                    var db = mongo.UpdateMany(filter, updated);
                    if (db.iFlg <= 0)
                    {
                        msg = oldls[i] + " 置换 " + newls[i] + " 失败";
                    }
                    else
                    {
                        val += oldls[i] + " 置换 " + newls[i] + " 成功  \r\n" ;
                    }
                }
                if (msg.Length == 0)
                {
                    flag = true;msg = val;
                }
            }
            return flag;
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="ls">参数list</param>
        /// <returns></returns>
        public int AddList(List<km_CollectCode> ls)
        {
            int ret = 0;
            try
            {
                DbMessage s = mongo.InsertMany<km_CollectCode>(ls);
                ret = 1;
            }
            catch
            {
            } 
            return ret;
        }
        /// <summary>
        /// 更新码状态
        /// </summary>
        /// <param name="order"></param>
        public void km_UpdateCollectedStatus(string order)
        {
            try
            {
                FilterDefinitionBuilder<BsonDocument> builder = new FilterDefinitionBuilder<BsonDocument>();
                FilterDefinition<BsonDocument> filter = builder.And(builder.Eq("lcode3", order), builder.Eq("upstatus", 1));


                var updated = Builders<BsonDocument>.Update.Set("upstatus", 2);
                var db = mongo.UpdateMany(filter, updated);
            }
            catch (Exception ex)
            { 
            
            }
        }

        public int Package(List<string> packls, string code,string tempbagcode)
        {
            int ret = 0;
            try
            {
                //var filter = Builders<BsonDocument>.Filter.Eq("lcode3", tempbagcode);
                //List<km_CollectCode> ls = mongo.QueryMany(Builders<km_CollectCode>.Filter.Where(x => x.lcode3.Equals(tempbagcode) && x.upstatus.Equals(0)));
                //DbMessage db = mongo.UpdateMany<km_CollectCode>(Builders<km_CollectCode>.Filter.Where(x => x.lcode3.Equals(tempbagcode) && x.upstatus.Equals(0)),
                //                     Builders<km_CollectCode>.Update.Set(x => x.lcode3, tempbagcode),true);

                FilterDefinitionBuilder<BsonDocument> builder = new FilterDefinitionBuilder<BsonDocument>();
                FilterDefinition<BsonDocument> filter = builder.And(builder.Eq("lcode3", tempbagcode), builder.Eq("upstatus", 0));


                var updated = Builders<BsonDocument>.Update.Set("lcode3", code).Set("upstatus", 1);
                var db = mongo.UpdateMany(filter, updated);
                if (db.iFlg > 0)
                {
                    //更新小码状态成功
                    ret = 1;
                }
            }
            catch (Exception ex)
            {
            }

            return ret;
        }

     

        public void DeleteData(List<string> packls)
        {
            foreach (string one in packls)
            {
                FilterDefinitionBuilder<BsonDocument> builder = new FilterDefinitionBuilder<BsonDocument>();
                FilterDefinition<BsonDocument> filter = builder.And(builder.Eq("lcode1", one), builder.Eq("upstatus", 0));
                mongo.DeleteMany(filter, true);
            }

        
        }

        public List<km_CollectCode> GetCollectedData(string order,int status)
        {

            FilterDefinitionBuilder<km_CollectCode> builderFilter = Builders<km_CollectCode>.Filter;
            FilterDefinition<km_CollectCode> filter;
            if (status != 3)
                filter = builderFilter.And(builderFilter.Eq("UploadOrder", order), builderFilter.Eq("upstatus", status));
            else
                filter = builderFilter.And(builderFilter.Eq("UploadOrder", order));
            // FilterDefinition<BsonDocument>(.Eq("UploadOrder", order) &Builders<BsonDocument>.Filter.Eq("upstatus",0)
            List< km_CollectCode> ls =  mongo.QueryMany<km_CollectCode>(filter,0);
            return ls;
        }
    }
}
