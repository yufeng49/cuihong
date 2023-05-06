using Entity.entity;
using System;
using System.Collections.Generic;

namespace Common
{

    #region 槟榔
    public class BetelNutRequest<T>
    {
        public string msg { get; set; }
        public string code { get; set; }
        public T data { get; set; }
    }

    public class BetelNutLogin
    {
        public string userId { get; set; }

        public string token { get; set; }
    }

    #endregion

    public class GetFactory
    {

        public string code;

        public List<Factory> data;

        public string msg;

        public object page;
    }

    public class Factory
    {
        private string id;
        private string updateBy;
        private string updateTime;
        private string createBy;
        private string createTime;
        private string remark;
        private string no;
        private string name;

        public string Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }

    }

    public class PolicyInfo
    {
        public string code;

        public List<Policy> data;

        public string msg;

    }

    public class Policy
    {

        public string id { get; set; }

        public string shortName { get; set; }

    }


    public class ReceiveModel
    {

        public string code;

        public Version data;

        public string msg;

        public object page;
    }


    public class Order
    {

        public string code;

        public List<OrderInfo> data;

        public string msg;

        public object page;

        public string id;


    }
    public class OrderOne
    {

        public string code;

        public OrderInfo data;

        public string msg;

        public object page;

        public string id;

    }

    public class PullTask
    {

        public string code;

        public Dictionary<string, List<string>> data;

        public string msg;

        public object page;

    }

    public class TaskInfo
    {
        public string pullId;

        public Dictionary<string, List<string>> qrCodeList;
    }


    public class OrderInfo
    {
        public string id;

        public string materialNo;

        public string createBy;

        public DateTime createTime;

        public string orderNum;

        public string customerName;

        public string wrapRatio;

        public string valuationUnitsNum;

        public string residueMinWarpNum;

        public string materialName;

        public string customerAddr;

        public string orderStatus;

        public int residueMaxWarpNum;

        public int sellUnitsNum;
    }

    public class Version
    {
        public string id;
        public string updateBy;
        public string updateTime;
        public string createBy;
        public string createTime;
        public string remark;
        public string clientType;
        public string status;
        public string versionUrl;
        public string updateContent;
        public string updateVersionNo;
        public string updateVersionCode;
        public string nowVersionNo;
        public string nowVersionCode;
        public string appCode;
    }
}
