using System.Collections.Generic;

namespace Entity.entity
{
    public class RequestData<T>
    {
        public string code;
        public T result;
        public T data;
        public string success;
        public string message;
        public string msg;
        public object timestamp;
    }
    public class LoginEntity
    {
        public Token token;
    }
    public class BillInfo
    {
        public string batchCode;
        public string billNo;
}
    public class Token
    {
        public string tokenValue;
    }
 
    public class Product {
        public long id;
        public string productCode { get; set; }
        public string productName { get; set; }
        public string productPhoto;
        public string productionCode;
        public string createBy;
        public string createTime;
        public string updateBy;

        public string updateTime;
        public int delFlag;
        public string remark;
        public object productBasicList;
    }

    public class ProductBasic
    {
        public long id { get; set; }
        public long productId { get; set; }
        public string productBasicCode { get; set; }
        public string productBasicName { get; set; }
        public int delFlag { get; set; }
        public string createBy { get; set; }
        public string createTime { get; set; }
    }
        public class RequstNoData
    {
        public string code { set; get; }
        public string success { set; get; }
        public string message { set; get; }

        public object result { set; get; }


    }

    /// <summary>
    /// 生产任务
    /// </summary>
    public class Createtask
    { 		
        public string batchCode { get; set; }
        public string billNo { get; set; }
        public string createBy { get; set; }
        public string createTime { get; set; }
        public string dateList { get; set; }
        public string dateTime { get; set; }
        public int delFlag { get; set; }
        public long id { get; set; }
        public string plantCode { get; set; }
        public string plantName { get; set; }
        public string productBasicCode { get; set; }
        public string productBasicId { get; set; }
        public string productBasicName { get; set; }
        public string productCode { get; set; }
        public long productId { get; set; }
        public string productName { get; set; }
        public string productionNumber { get; set; }
        public string remark { get; set; }
        public int sendStatus { get; set; }
        public int taskNumber { get; set; }
        public int taskStatus { get; set; }
        public string updateBy { get; set; }
        public string updateTime { get; set; }
        public string workshopCode { get; set; }
        public string workshopName { get; set; }
    }



}
