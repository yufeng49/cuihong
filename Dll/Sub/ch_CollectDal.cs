using Dll.Base;
using Entity.entity;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDbSpider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dll.Sub
{
    public class ch_CollectDal
    {
        MongoDbSpider.MongoHelper mongo = new MongoDbSpider.MongoHelper(GenerateSql.MongoDBconnectionString, "chdb", "ch_Collect");
        MongoDbSpider.MongoHelper mongo_Upload = new MongoDbSpider.MongoHelper(GenerateSql.MongoDBconnectionString, "chdb", "ch_Upload");

        public bool Addls(List<ch_Collect> ls)
        {

            int ret = 0;
            try
            {
               mongo.InsertManyAsync<ch_Collect>(ls);
                ret = 1;
            }
            catch
            {
            }
            return ret >0;
        }

        public void AddUploadStr(ch_Uploadstr upstr)
        {
            mongo_Upload.InsertAsync(upstr);
        }


        public List<ch_Uploadstr> GetUploadls() {

           return mongo_Upload.QueryMany<ch_Uploadstr>();
        }


        /// <summary>
        /// 修改码状态，并且删除上传
        /// </summary>
        /// <param name="chup"></param>
        /// <returns></returns>
        public bool deluploadmodifystatus(ch_Uploadstr chup)
        {
            bool flag = false;

            try
            {
                FilterDefinitionBuilder<BsonDocument> builder = new FilterDefinitionBuilder<BsonDocument>();
                FilterDefinition<BsonDocument> filter = builder.And(builder.Eq("parentcode", chup.fcode));
                var updated = Builders<BsonDocument>.Update.Set("codestatus", 1);
                var db = mongo.UpdateMany(filter, updated);

                var ss = mongo_Upload.DeleteOne<ch_Uploadstr>("fcode", chup.fcode, true);
                flag = true;
            }
            catch { 
            
            }


            return flag;
            //  mongo_Upload.DeleteOne("")
        }
    }
}