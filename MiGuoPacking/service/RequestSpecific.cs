using Common.Utils;
using MiGuoPacking.Tool;
using Server;
using Server.Base;
using Server.entity;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MiGuoPacking.service
{
    public class RequestSpecific
    {
        private static string headerUrl = ConfigHelper.GetValue("Url", "0");
        private static string Api = ConfigHelper.GetValue("Api", "0");
        private static string logPath = Application.StartupPath + "\\Response";
        private static string logPathdt = Application.StartupPath + "\\Data";
        public static LoginEntity Login(string url, string username, string pwd)
        {
            try
            {
                //    "https://q.cqfeima.cn/qrcode/client/login?password=123456&username=collect1"

                string api = Api == "0" ? "" : Api;
                string submitUrl = headerUrl + api + url + "?password=" + pwd + "&username=" + username;
                string submitParms = "";
                string result = RequestServe.GetRequestPost<LoginEntity>(submitUrl, submitParms, "", logPath, "登录");

                var rm = JsonHelper.DeserializeObject<ResponseEntity<LoginEntity>>(result);

                if (rm.code == "200" || rm.code == "0")
                {
                    return rm.data;
                }
                MessageBox.Show(rm.message);
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(" 登录失败:" + ex.Message);
                return null;
            }
        }

        public static List<ProductEntity> GetProductPost(string url, object parms, string token)
        {
            try
            {
                BoxShow.OpenLoadProcessBar();
                string submitUrl = headerUrl + Api + url;
                string submitParms = JsonHelper.SerializeObject(parms);
                string result = RequestServe.GetRequestPost<List<ProductEntity>>(submitUrl, submitParms, token, logPath, "获取产品");
                BoxShow.CloseLoadProcessBar();
                var rm = JsonHelper.DeserializeObject<ResponseEntity<List<ProductEntity>>>(result);

                if (rm.code == "200" || rm.code == "0")
                {
                    return rm.data;
                }
                MessageBox.Show(rm.message);
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(" 获取失败:" + ex.Message);
                return null;
            }
        }

        public static List<ProductEntity> GetProduct(string url, Dictionary<string, string> dic, string token)
        {
            string str = "";
            if (dic.Count > 0)
            {
                str += "?";
                foreach (var item in dic)
                {
                    str += item.Key + "=" + item.Value + "&";
                }
                str = str.Substring(0, str.Length - 1);
            }

            string submitUrl = headerUrl + Api.Replace("/jeecg", "") + url + str;
            string result = RequestServe.GetRequestGet<List<ProductEntity>>(submitUrl, token, logPath, "获取产品");
            var rm = JsonHelper.DeserializeObject<ResponseEntity<List<ProductEntity>>>(result);

            if (rm.code == "200" || rm.code == "0")
            {
                return rm.data;
            }
            MessageBox.Show(rm.message);
            return rm.data;
        }

        public static List<ProductSpecEntity> GetProductSpecPost(string url, object parms, string token)
        {
            try
            {
                BoxShow.OpenLoadProcessBar();
                string submitUrl = headerUrl + Api + url;
                string submitParms = JsonHelper.SerializeObject(parms);
                string result = RequestServe.GetRequestPost<List<ProductSpecEntity>>(submitUrl, submitParms, token, logPath, "获取规格");
                BoxShow.CloseLoadProcessBar();
                var rm = JsonHelper.DeserializeObject<ResponseEntity<List<ProductSpecEntity>>>(result);

                if (rm.code == "200" || rm.code == "0")
                {
                    return rm.data;
                }
                MessageBox.Show(rm.message);
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(" 获取失败:" + ex.Message);
                return null;
            }
        }

        public static List<ProductSpecEntity> GetProductSpec(string url, Dictionary<string, string> dic, string token)
        {
            string str = "";
            if (dic.Count > 0)
            {
                str += "?";
                foreach (var item in dic)
                {
                    str += item.Key + "=" + item.Value + "&";
                }
                str = str.Substring(0, str.Length - 1);
            }

            string submitUrl = headerUrl + Api.Replace("/jeecg", "") + url + str;
            string result = RequestServe.GetRequestGet<List<ProductSpecEntity>>(submitUrl, token, logPath, "获取班组");
            var rm = JsonHelper.DeserializeObject<ResponseEntity<List<ProductSpecEntity>>>(result);

            if (rm.code == "200" || rm.code == "0")
            {
                return rm.data;
            }
            MessageBox.Show(rm.message);
            return rm.data;
        }

        public static List<TeamEntity> GetTeam(string url, Dictionary<string, string> dic, string token)
        {
            string str = "";
            if (dic.Count > 0)
            {
                str += "?";
                foreach (var item in dic)
                {
                    str += item.Key + "=" + item.Value + "&";
                }
                str = str.Substring(0, str.Length - 1);
            }

            string submitUrl = headerUrl + Api + url + str;
            string result = RequestServe.GetRequestGet<List<TeamEntity>>(submitUrl, token, logPath, "获取班组");
            var rm = JsonHelper.DeserializeObject<ResponseEntity<List<TeamEntity>>>(result);

            if (rm.code == "200" || rm.code == "0")
            {
                return rm.data;
            }
            MessageBox.Show(rm.message);
            return rm.data;
        }

        public static ResponseEntity<List<string>> GetCodesPost(string url)
        {
            try
            {
                BoxShow.OpenLoadProcessBar();
                string submitUrl = headerUrl + url;
                string result = RequestServe.GetRequestGet<List<string>>(submitUrl, "", logPath, "获取父级码");
                BoxShow.CloseLoadProcessBar();
                var rm = JsonHelper.DeserializeObject<ResponseEntity<List<string>>>(result);

                return rm;

            }
            catch (Exception ex)
            {
                MessageBox.Show(" 获取失败:" + ex.Message);
                return new ResponseEntity<List<string>>() { code = "-1", msg = ex.Message };
            }
        }
        public static T GetProduct<T>(string url, object parms, string token, string interfaceName)
        {

            string submitUrl = headerUrl + url;
            string submitParms = JsonHelper.SerializeObject(parms);
            string result = RequestServe.GetRequestPost<T>(submitUrl, submitParms, token, logPath, interfaceName);
            var rm = JsonHelper.DeserializeObject<ResponseEntity<T>>(result);

            if (rm.code == "200" || rm.code == "0")
            {
                return rm.data;
            }
            else
            {
                MessageBox.Show(rm.msg);
                return default;
            }
        }
        public static Dictionary<string, List<string>> GetCodes(string url, Dictionary<string, string> dic, string token)
        {
            string str = "";
            if (dic.Count > 0)
            {
                str += "?";
                foreach (var item in dic)
                {
                    str += item.Key + "=" + item.Value + "&";
                }
                str = str.Substring(0, str.Length - 1);
            }

            string submitUrl = headerUrl + Api.Replace("/jeecg", "") + url + str;
            string result = RequestServe.GetRequestGet<List<ProductSpecEntity>>(submitUrl, token, logPath, "获取班组");
            var rm = JsonHelper.DeserializeObject<ResponseEntity<Dictionary<string, List<string>>>>(result);

            if (rm.code == "200" || rm.code == "0")
            {
                return rm.data;
            }
            MessageBox.Show(rm.message);
            return rm.data;
        }

        public static ResponseEntity<string> Submit(string url, string parms, string token)
        {
            try
            {
                // BoxShow.OpenLoadProcessBar();
                string submitUrl = headerUrl + url;
                string result = RequestServe.GetRequestPost<string>(submitUrl, parms, token, logPathdt, "提交数据");
                // BoxShow.CloseLoadProcessBar();
                var rm = JsonHelper.DeserializeObject<ResponseEntity<string>>(result);
                return rm;
            }
            catch (Exception ex)
            {
                return new ResponseEntity<string> { code = "201", message = ex.Message };
            }
        }

        public static ReprintPageEntity GetApplyFor(string url, object parms, string token)
        {
            try
            {
                BoxShow.OpenLoadProcessBar();
                string submitUrl = headerUrl + Api + url;
                string submitParms = JsonHelper.SerializeObject(parms);
                string result = RequestServe.GetRequestPost<List<ReprintEntity>>(submitUrl, submitParms, token, logPath, "获取补打申请");
                BoxShow.CloseLoadProcessBar();
                var rm = JsonHelper.DeserializeObject<ResponseEntity<ReprintPageEntity>>(result);

                if (rm.code == "200" || rm.code == "0")
                {
                    return rm.data;
                }
                MessageBox.Show(rm.message);
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(" 获取失败: " + ex.Message);
                return null;
            }
        }
        #region 施特优
        public static ResponseEntity<string> SubmitBy(string url, string parms, string token)
        {
            try
            {
                // BoxShow.OpenLoadProcessBar();
                string submitUrl = url;
                string result = RequestServe.GetRequestPost<string>(submitUrl, parms, token, logPathdt, "提交数据");
                // BoxShow.CloseLoadProcessBar();
                var rm = JsonHelper.DeserializeObject<ResponseEntity<string>>(result);
                return rm;
            }
            catch (Exception ex)
            {
                return new ResponseEntity<string> { code = "201", message = ex.Message };
            }
        }
        #endregion
    }
}
