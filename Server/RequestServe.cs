using Common;
using Common.Utils;
using System;
using System.Collections.Generic;

namespace Server
{
    public static class RequestServe
    {

        public static string GetRequestPost<T>(string submitUrl, string submitParms, string token, string logPath, string interfaceName)
        {
            string result = Request.Post(submitUrl, submitParms, token, "Authorization");
            string logInfo = DateTime.Now.ToString() + "\r   接口名称：" + interfaceName + "\r   请求方式：Post\r   调用接口：" + submitUrl + "\r   参数：" + submitParms + "\r   结果：" + result;
            LogHelper.WriteLog(logPath, logInfo);
            return result;
        }

        public static string GetRequestPostForm<T>(string submitUrl, List<LoginFormItemModel> submitParms, string token, string logPath, string interfaceName)
        {
            string result = Request.PostForm(submitUrl, submitParms);
            string logInfo = DateTime.Now.ToString() + "\r   接口名称：" + interfaceName + "\r   请求方式：Post\r   调用接口：" + submitUrl + "\r   参数：" + submitParms + "\r   结果：" + result;
            LogHelper.WriteLog(logPath, logInfo);
            return result;
        }

        public static string GetRequestGet<T>(string submitUrl, string token, string logPath, string interfaceName)
        {
            string result = Request.Get(submitUrl, token);
            string logInfo = DateTime.Now.ToString() + "\r   接口名称：" + interfaceName + "\r   请求方式：Get\r   调用接口：" + submitUrl + "\r   参数：" + "" + "\r   结果：" + result;
            LogHelper.WriteLog(logPath, logInfo);
            return result;
        }
    }
}
