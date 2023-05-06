using Common.Utils;
using Entity.entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Common
{
    public static class ResponseUtil
    {
        private static string urll = ConfigHelper.GetValue("RequestUrl", "https://z.bzlsp.cn");
        private static string LogPathLog = Application.StartupPath + "\\RequestData";
        private static string token = "eShengCement@123";
        private static string tokenname = "Blade-Auth";

        
        #region 槟榔

        public static PolicyInfo GetPolicy(string url, Dictionary<string, string> dic, string token)
        {
            try
            {
                string str = "?";
                foreach (var item in dic)
                {
                    str += item.Key + "=" + item.Value + "&";
                }
                str = str.Substring(0, str.Length - 1);
                //  string parms = JsonConvert.SerializeObject(obj);
                string post = urll + url + str;
                string result = Request.Get(post, token);
                LogHelper.WriteLog(LogPathLog, DateTime.Now + "  " + result);
                return JsonConvert.DeserializeObject<PolicyInfo>(result);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(LogPathLog, DateTime.Now + "  提交失败" + ex.Message);
                return new PolicyInfo { msg = "网络请求失败", code = "201" };
            }

        }

      

        #endregion


        #region 峨胜水泥
        /// <summary>
        /// 获取入库信息
        /// </summary>
        /// <param name="hdno">硬件号</param>
        /// <returns></returns>
        public static RequestData<EshengEntity> CheckOrder(string url, string hdno, string previousQrCode)
        {
            try
            {
                string parms = "";
                //"{\"hdNo\":\"" + hdno + "\"}";
                string post = urll + url + "?hdNo=" + System.Net.WebUtility.UrlEncode(hdno) + (previousQrCode.Length > 0 ? "&previousQrCode=" + previousQrCode : "");
                string result = Request.Get(post, token, tokenname, false);
                LogHelper.WriteLog(LogPathLog, DateTime.Now + "获取打印二维码  请求参数:" + post + ",请求结果：" + result);
                return JsonConvert.DeserializeObject<RequestData<EshengEntity>>(result);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(LogPathLog, DateTime.Now + "获取打印二维码-异常  提交失败" + ex.Message);
                return new RequestData<EshengEntity> { message = "网络请求失败", code = "201" };
            }
        }
        public static RequestData<EshengEntity> FromOrder(string url, string hdno, string qrCode)
        {
            try
            {
                //"{\"hdNo\":\"" + hdno + "\"}";
                string post = urll + url + "?hdNo=" + System.Net.WebUtility.UrlEncode(hdno) + (qrCode.Length > 0 ? "&qrCode=" + qrCode : "");
                string result = Request.Get(post, token, tokenname, false);
                LogHelper.WriteLog(LogPathLog, DateTime.Now + "获取打印二维码  请求参数:" + post + ",请求结果：" + result);
                return JsonConvert.DeserializeObject<RequestData<EshengEntity>>(result);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(LogPathLog, DateTime.Now + "获取打印二维码-异常  提交失败" + ex.Message);
                return new RequestData<EshengEntity> { message = "网络请求失败", code = "201" };
            }
        }

      

        public static RequestData<EshengEntity> CheckOrder(string url, string hdno)
        {
            return CheckOrder(url, hdno, "");
        }

        /// <summary>
        /// 采集提交
        /// </summary>
        /// <param name="url">接口地址</param>
        /// <param name="hdNo">硬件号</param>
        /// <param name="qrCode">二维码</param>
        /// <returns></returns>
        public static RequestData<object> CollectData(string url, string hdNo, string qrCode, string fail)
        {


            try
            {
                string parms = "?hdNo=" + System.Net.WebUtility.UrlEncode(hdNo) + "&qrCode=" + qrCode + fail;
                string post = urll + url + parms;
                string result = Request.Get(post, token, tokenname, false);
                LogHelper.WriteLog(LogPathLog, DateTime.Now + "采集提交  请求参数:" + parms + ",请求结果：" + result);
                RequestData<object> results = JsonConvert.DeserializeObject<RequestData<object>>(result);

                return results;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(LogPathLog, DateTime.Now + "采集提交-异常  ,异常信息：" + ex.Message);

                return new RequestData<object>() { code = "100", message = "异常：" + ex.Message };
            }
        }


        public static RequestData<EshengCheckout> CheckOutOrder(string url, string hdNo, string dispatchOrderId)
        {
            string parms = "?hdNo=" + System.Net.WebUtility.UrlEncode(hdNo) + "&dispatchOrderId=" + dispatchOrderId;
            try
            {
                string post = urll + url + parms;
                string result = Request.Get(post, token, tokenname, false);
                RequestData<EshengCheckout> results = JsonConvert.DeserializeObject<RequestData<EshengCheckout>>(result);
                if (!(results.code.Equals("200") || results.code.Equals("100011"))) LogHelper.WriteLog(LogPathLog, DateTime.Now + "获取小票  请求参数:" + parms + ",请求结果：" + result);
                return results;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(LogPathLog, DateTime.Now + "获取小票-异常  ,参数：" + parms + "异常信息：" + ex.Message);
                return new RequestData<EshengCheckout>() { code = "100", message = "异常：" + ex.Message };
            }
        }
        public static bool CheckStatus(string url, string dispatchOrderId, string hdNo, List<string> stockOut)
        {
            try
            {
                string qr = JsonConvert.SerializeObject(stockOut);
                string parms = "{\"dispatchOrderId\": \"" + dispatchOrderId + "\",\"hdNo\": \"" + hdNo + "\",\"qrCodeList\": " + qr + "}";
                string post = urll + url;
                string result = Request.Get(post, token, "Blade-Auth", true);
                LogHelper.WriteLog(LogPathLog, DateTime.Now + "状态检测  请求参数:" + post + ",请求结果：" + result);

                if (result.Contains("异常"))
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static RequestData<object> submitConfirm(string url, string dispatchOrderId, string hdNo, List<string> stockOut)
        {
            try
            {
                string qr = JsonConvert.SerializeObject(stockOut);
                string parms = "{\"dispatchOrderId\": \"" + dispatchOrderId + "\",\"hdNo\": \"" + hdNo + "\",\"qrCodeList\": " + qr + "}";
                string post = urll + url;
                string result = Request.Post(post, parms, token, tokenname);
                LogHelper.WriteLog(LogPathLog, DateTime.Now + "确认出库  请求参数:" + parms + ",请求结果：" + result);
                RequestData<object> results = JsonConvert.DeserializeObject<RequestData<object>>(result);

                return results;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(LogPathLog, DateTime.Now + "确认出库-异常  ,异常信息：" + ex.Message);

                return new RequestData<object>() { code = "100", message = "异常：" + ex.Message };
            }
        }

        public static RequestData<stockOutinfo> stockOut(string url, string dispatchOrderId, string hdNo, string stockOut)
        {
            try
            {
                string parms = "{\"dispatchOrderId\": \"" + dispatchOrderId + "\",\"hdNo\": \"" + hdNo + "\",\"qrCode\":\"" + stockOut + "\"}";
                string post = urll + url;
                string result = Request.Post(post, parms, token, tokenname);
                LogHelper.WriteLog(LogPathLog, DateTime.Now + "单个出库  请求参数:" + parms + ",请求结果：" + result);
                RequestData<stockOutinfo> results = JsonConvert.DeserializeObject<RequestData<stockOutinfo>>(result);

                return results;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(LogPathLog, DateTime.Now + "单个出库-异常  ,异常信息：" + ex.Message);

                return new RequestData<stockOutinfo>() { code = "100", message = "异常：" + ex.Message };
            }
        }

        public static RequestData<object> delStockOut(string delStockOut, string qrcode, string hdNo)
        {
            try
            {
                string parms = "[\"" + qrcode + "\"]";
                string post = urll + delStockOut;
                string result = Request.Post(post, parms, token, tokenname);
                LogHelper.WriteLog(LogPathLog, DateTime.Now + "删除单个出库  请求参数:" + parms + ",请求结果：" + result);
                RequestData<object> results = JsonConvert.DeserializeObject<RequestData<object>>(result);

                return results;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(LogPathLog, DateTime.Now + "删除单个出库-异常  ,异常信息：" + ex.Message);

                return new RequestData<object>() { code = "100", message = "异常：" + ex.Message };
            }
        }
        #endregion

        #region 康美
        /// <summary>
        /// 拉取康美产品
        /// </summary>
        /// <returns></returns>
        public static RequestData<List<KangmeiEntity>> GetKangmeiProd(string str) {
            RequestData<List<KangmeiEntity>> ls = new RequestData<List<KangmeiEntity>>();
            string post = urll + str;
            string result =   Request.Get(post, token);

            try
            {
                LogHelper.WriteLog(LogPathLog, DateTime.Now + "  拉取产品接口，返回值：" + result);
                ls= JsonConvert.DeserializeObject<RequestData<List<KangmeiEntity>>>(result);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(LogPathLog, DateTime.Now + "  拉取产品接口异常，返回值：" + result +"--异常信息:"+ex.Message);
            }
            return ls;
        }

        public static string km_GetCode(string getCode, string prodId)
        {
            RequestData<string> ls = new RequestData<string>();
            string post = urll + getCode+prodId;
            string result = Request.Get(post, token);
            string resultval = "";
            try
            {
                LogHelper.WriteLog(LogPathLog, DateTime.Now + "  拉取箱码接口，返回值：" + result);
                ls = JsonConvert.DeserializeObject < RequestData<string>>(result);
                if (ls.code.Equals("0"))
                {
                    resultval = ls.result;
                }
                else
                {
                    resultval = "0";
                }

            }
            catch (Exception ex)
            {
                resultval = "-1";
                LogHelper.WriteLog(LogPathLog, DateTime.Now + "  拉取箱码接口异常，返回值：" + result + "--异常信息:" + ex.Message);
            }
            return resultval;
        }
        public static int km_UpLoad(string urladd,string Upinfo)
        {
            string result = "";
            try
            {
                string post = urll + urladd;
                result = Request.Post(post, Upinfo);
                LogHelper.WriteLog(LogPathLog, DateTime.Now + "  上传接口 参数：" + Upinfo + "，返回值：" + result);
                RequstNoData ss = JsonConvert.DeserializeObject<RequstNoData>(result);
                if (ss.code.Equals("0"))
                {
                    return 1;
                }
                else
                {
                    return 0;

                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(LogPathLog, DateTime.Now + "  上传接口 参数：" + Upinfo + "结果："+ result + "，异常：" + ex.Message);
                return 0;
            }


        }
        #endregion


        #region 翠宏
        public static string ch_Login(string url,string acct,string pwd) {

            string ret = "";
            string param = "{\"password\": \"" + pwd + "\",\"username\": \"" + acct + "\"}"; string post = urll + url;
            try
            {
                ret = Request.Post(post, param,"");
                LogHelper.WriteLog(LogPathLog, DateTime.Now + "  登录 参数：" + param + "，返回值：" + ret);
                RequestData<LoginEntity> ss = JsonConvert.DeserializeObject<RequestData<LoginEntity>>(ret);
                if (ss.code.Equals("200"))
                {
                    ret = ss.result.token.tokenValue;
                }
                else
                {
                    ret = "#" + ss.message;
                }

            }
            catch (Exception ex)
            {
                ret = "#" + ex.Message;
                LogHelper.WriteLog(LogPathLog, DateTime.Now + "  登录 参数：" + param + "结果：" + ret + "，异常：" + ex.Message);
            
            }
            return ret;
        }
        public static List<Product>  getProduct(string url, string token,out string msg)
        {
            msg = "";
            string param = "{}";
            string post = urll + url;
            List<Product> ls = new List<Product>();
            try
            {
               string ret = Request.Post(post, param,token);
                LogHelper.WriteLog(LogPathLog, DateTime.Now + "  查询产品 参数：" + param + "，返回值：" + ret);
                RequestData<List<Product>> ss = JsonConvert.DeserializeObject<RequestData<List<Product>>>(ret);
                if (ss.code.Equals("200"))
                {
                    ls = ss.result;
                    msg = "成功";
                }
                else
                {
                    msg = ss.message;
                }

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                LogHelper.WriteLog(LogPathLog, DateTime.Now + "  查询产品 参数：" + param +  "，异常：" + ex.Message);

            }
            return ls;
        }
        public static List<ProductBasic> getProductBasic(string url, string token, long productid, out string msg)
        {
            msg = "";
            string param = "{\"productId\":\""+ productid + "\"}";
            string post = urll + url;
            List<ProductBasic> ls = new List<ProductBasic>();
            try
            {
                string ret = Request.Post(post, param, token);
                LogHelper.WriteLog(LogPathLog, DateTime.Now + "  查询产品规格 参数：" + param + "，返回值：" + ret);
                RequestData<List<ProductBasic>> ss = JsonConvert.DeserializeObject<RequestData<List<ProductBasic>>>(ret);
                if (ss.code.Equals("200"))
                {
                    ls = ss.result;
                    msg = "成功";
                }
                else
                {
                    msg = ss.message;
                }

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                LogHelper.WriteLog(LogPathLog, DateTime.Now + "  查询产品规格 参数：" + param + "，异常：" + ex.Message);

            }
            return ls;
        }
        public static string getBillNo(string url, string token, string batch, string plantCode, string plantName, string productBasicCode, string productBasicName, string productCode, string productName, int num, string workshopCode, string workshopName)
        {
            string ret = "";
            string param = "{\"batchCode\": \""+batch+"\",\"plantCode\": \""+ plantCode + "\",\"plantName\": \""+ plantName + "\",\"productBasicCode\": \""+ productBasicCode + "\",\"productBasicName\": \""+ productBasicName + "\",\"productCode\":\""+ productCode+"\",\"productName\":\""+ productName + "\",\"taskNumber\": "+num+",\"workshopCode\": \""+ workshopCode + "\",\"workshopName\":\""+ workshopName + "\"}";
            string post = urll + url;
            try
            {
                ret = Request.Post(post, param, token);
                LogHelper.WriteLog(LogPathLog, DateTime.Now + "  获取单据 参数：" + param + "，返回值：" + ret);
                RequestData<BillInfo> ss = JsonConvert.DeserializeObject<RequestData<BillInfo>>(ret);
                if (ss.code.Equals("200"))
                {
                    ret = ss.result.billNo;
                }
                else
                {
                    ret = "#" + ss.message;
                }

            }
            catch (Exception ex)
            {
                ret = "#" + ex.Message;
                LogHelper.WriteLog(LogPathLog, DateTime.Now + "  获取单据 参数：" + param + "结果：" + ret + "，异常：" + ex.Message);

            }
            return ret;
        }

        public static Dictionary<string, List<string>> ch_GetCode(string url, string token, out string msg, int count)
        {
            Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();
            msg = "";

            string param = "{\"maxWarpNum\": " + count + ",\"warpRate\": { \"1\":\"0\",\"2\":\"1\",\"3\":\"1\"} }";
            string post = urll + url;
            try
            {
                string ret = Request.Post(post, param, token);
                LogHelper.WriteLog(LogPathLog, DateTime.Now + "  拉取父级码 参数：" + param + "，返回值：" + ret);
                RequestData<Dictionary<string, List<string>>> ss = JsonConvert.DeserializeObject<RequestData<Dictionary<string, List<string>>>>(ret);
                if (ss.code.Equals("200"))
                {
                    dict.Add("box", ss.result["2"]);//箱码
                    dict.Add("duo", ss.result["3"]);//垛码
                    msg = "成功";
                }
                else
                {
                    msg = "#" + ss.message;
                }

            }
            catch (Exception ex)
            {
                msg = "#" + ex.Message;
                LogHelper.WriteLog(LogPathLog, DateTime.Now + "  拉取父级码 参数：" + param + "，异常：" + ex.Message);

            }
            return dict;
        }
        public static List<Createtask> GetTask(string url , Createtask ct,string token,out string res )
        {
            List<Createtask> ls = new List<Createtask>();
             res = "";
            string param = JsonConvert.SerializeObject(ct);
            string post = urll + url;
            try
            {
              string  ret = Request.Post(post, param, token);
                LogHelper.WriteLog(LogPathLog, DateTime.Now + "  获取任务列表 参数：" + param  +" 结果:"+ ret);
                RequestData<List<Createtask>> ss = JsonConvert.DeserializeObject<RequestData<List<Createtask>>>(ret);
                if (ss.code.Equals("200"))
                {
                    res = "成功";
                       ls = ss.result;
                }
                else
                {
                  res = ss.message;
                }

            }
            catch (Exception ex)
            {
                res = "#" + ex.Message;
                LogHelper.WriteLog(LogPathLog, DateTime.Now + "  获取任务列表 参数：" + param + "结果：" + res + "，异常：" + ex.Message);

            }
            return ls;
        }


        public static bool ch_UpLoad(string url, string param, string token)
        {
            List<Createtask> ls = new List<Createtask>();
            bool flag = false;
            string post = urll + url;
            string ret;
            try
            {
                ret = Request.Post(post, param, token);
                LogHelper.WriteLog(LogPathLog, DateTime.Now + "  采集关联 参数：" + param +",结果："+ret);
                RequestData<string>  ss = JsonConvert.DeserializeObject<RequestData<string>>(ret);
                if (ss.code.Equals("200"))
                {
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(LogPathLog, DateTime.Now + "  采集关联 参数：" + param + "，异常：" + ex.Message);

            }
            return flag;
        }
        #endregion 
    }
}
