using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Common
{
    public class Request
    {
        public static string Post(string url, string parm, string token, string tokenName)
        {
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/json;charset=UTF-8";
            if (!token.Equals(""))
            {
                req.Headers.Add(tokenName, token);
            }
            byte[] data = Encoding.UTF8.GetBytes(parm);//把字符串转换为字节
            req.ContentLength = data.Length; //请求长度

            try
            {
                using (Stream reqStream = req.GetRequestStream()) //获取
                {
                    reqStream.Write(data, 0, data.Length);//向当前流中写入字节
                    reqStream.Close(); //关闭当前流
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            try
            {
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse(); //响应结果
                Stream stream = resp.GetResponseStream();
                //获取响应内容
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    result = reader.ReadToEnd();
                }
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public static string Post(string url, string parm, string token)
        {
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/json;charset=UTF-8";
            if (!token.Equals(""))
            {
                req.Headers.Add("Token", token);
            }
            byte[] data = Encoding.UTF8.GetBytes(parm);//把字符串转换为字节
            req.ContentLength = data.Length; //请求长度

            try
            {
                using (Stream reqStream = req.GetRequestStream()) //获取
                {
                    reqStream.Write(data, 0, data.Length);//向当前流中写入字节
                    reqStream.Close(); //关闭当前流
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            try
            {
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse(); //响应结果
                Stream stream = resp.GetResponseStream();
                //获取响应内容
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    result = reader.ReadToEnd();
                }
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }


        public static string Post(string url, Dictionary<string, string> dic, string token)
        {
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/json;charset=UTF-8";
            if (!token.Equals(""))
            {
                req.Headers.Add("token", token);
            }
            StringBuilder buffer = new StringBuilder();
            int i = 0;
            foreach (string key in dic.Keys)
            {
                //if (i > 0)
                //{
                //    buffer.AppendFormat("&{0}={1}", key, dic[key]);
                //}
                //else
                {
                    buffer.AppendFormat("{0}={1}", key, dic[key]);
                }
                i++;

            }
            byte[] data = Encoding.UTF8.GetBytes(buffer.ToString());//把字符串转换为字节
            req.ContentLength = data.Length; //请求长度

            try
            {
                using (Stream reqStream = req.GetRequestStream()) //获取
                {
                    reqStream.Write(data, 0, data.Length);//向当前流中写入字节
                    reqStream.Close(); //关闭当前流
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            try
            {
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse(); //响应结果
                Stream stream = resp.GetResponseStream();
                //获取响应内容
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    result = reader.ReadToEnd();
                }
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public static string Post(string url, string parm)
        {
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/json;charset=UTF-8";
            byte[] data = Encoding.UTF8.GetBytes(parm);//把字符串转换为字节
            req.ContentLength = data.Length; //请求长度

            try
            {
                using (Stream reqStream = req.GetRequestStream()) //获取
                {
                    reqStream.Write(data, 0, data.Length);//向当前流中写入字节
                    reqStream.Close(); //关闭当前流
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            try
            {
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse(); //响应结果.
                                                                           //  resp.StatusCode 
                Stream stream = resp.GetResponseStream();
                //获取响应内容
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    result = reader.ReadToEnd();
                }
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public static string Get(string url, string token)
        {
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Headers.Add("Token", token);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            try
            {
                //获取内容
                using (StreamReader reader = new StreamReader(stream))
                {
                    result = reader.ReadToEnd();
                }
            }
            finally
            {
                stream.Close();
            }
            return result;
        }
        public static string Get(string url, string token, string tokenname, bool flag)
        {
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            if (tokenname.Length > 0)
            {
                req.Headers.Add(tokenname, token);
            }
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            try
            {
                //获取内容
                using (StreamReader reader = new StreamReader(stream))
                {
                    result = reader.ReadToEnd();
                }
            }
            finally
            {
                stream.Close();
            }
            return result;
        }

        public static string Get(string url, string ReqInfo, string token, string tokenName)
        {
            //ServicePointManager.ServerCertificateValidationCallback += ValidateServerCertificate;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.CookieContainer = new CookieContainer();
            //request.Headers.Add("userName", "abc");  //设置信息头用户名
            //request.Headers.Add("password", "123456"); //设置信息头密码
            if (!token.Equals(""))
            {
                request.Headers.Add(tokenName, token);
            }
            request.KeepAlive = true;
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            //添加发送数据
            Encoding encoding = Encoding.GetEncoding("utf-8");
            if (!ReqInfo.Equals(""))
            {
                ReqInfo = "request=" + ReqInfo;
                byte[] postData = encoding.GetBytes(ReqInfo);
                request.ContentLength = postData.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(postData, 0, postData.Length);
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //获取返回数据
            Stream responseStream = response.GetResponseStream();
            if (responseStream == null) return "";
            StreamReader streamReader = new StreamReader(responseStream, encoding);
            string retString = streamReader.ReadToEnd();
            streamReader.Close();
            responseStream.Close();
            return retString;
        }

        public static string Get(string url, string ReqInfo, string token)
        {
            //ServicePointManager.ServerCertificateValidationCallback += ValidateServerCertificate;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.CookieContainer = new CookieContainer();
            //request.Headers.Add("userName", "abc");  //设置信息头用户名
            //request.Headers.Add("password", "123456"); //设置信息头密码
            if (!token.Equals(""))
            {
                request.Headers.Add("token", token);
            }
            request.KeepAlive = true;
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            //添加发送数据
            Encoding encoding = Encoding.GetEncoding("utf-8");
            if (!ReqInfo.Equals(""))
            {
                ReqInfo = "request=" + ReqInfo;
                byte[] postData = encoding.GetBytes(ReqInfo);
                request.ContentLength = postData.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(postData, 0, postData.Length);
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //获取返回数据
            Stream responseStream = response.GetResponseStream();
            if (responseStream == null) return "";
            StreamReader streamReader = new StreamReader(responseStream, encoding);
            string retString = streamReader.ReadToEnd();
            streamReader.Close();
            responseStream.Close();
            return retString;
        }

        /// <summary>
        /// 使用Post方法获取字符串结果
        /// </summary>
        /// <param name="url"></param>
        /// <param name="formItems">Post表单内容</param>
        /// <param name="cookieContainer"></param>
        /// <param name="timeOut">默认20秒</param>
        /// <param name="encoding">响应内容的编码类型（默认utf-8）</param>
        /// <returns></returns>
        public static string PostForm(string url, List<LoginFormItemModel> formItems, CookieContainer cookieContainer = null, string refererUrl = null, Encoding encoding = null, int timeOut = 20000)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                #region 初始化请求对象
                request.Method = "POST";
                request.Timeout = timeOut;
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.57 Safari/537.36";
                request.Headers.Add("Authorization", "Basic Y2xpZW50OmNsaWVudA==");
                if (!string.IsNullOrEmpty(refererUrl))
                    request.Referer = refererUrl;
                if (cookieContainer != null)
                    request.CookieContainer = cookieContainer;

                #endregion

                string boundary = "----" + DateTime.Now.Ticks.ToString("x");//分隔符
                request.ContentType = string.Format("multipart/form-data; boundary={0}", boundary);
                //请求流
                var postStream = new MemoryStream();
                #region 处理Form表单请求内容
                //是否用Form上传文件
                var formUploadFile = formItems != null && formItems.Count > 0;
                if (formUploadFile)
                {
                    //文件数据模板
                    string fileFormdataTemplate =
                        "\r\n--" + boundary +
                        "\r\nContent-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"" +
                        "\r\nContent-Type: application/octet-stream" +
                        "\r\n\r\n";
                    //文本数据模板
                    string dataFormdataTemplate =
                        "\r\n--" + boundary +
                        "\r\nContent-Disposition: form-data; name=\"{0}\"" +
                        "\r\n\r\n{1}";
                    foreach (var item in formItems)
                    {
                        string formdata = null;

                        //上传文本
                        formdata = string.Format(
                            dataFormdataTemplate,
                            item.Key,
                            item.Value);


                        //统一处理
                        byte[] formdataBytes = null;
                        //第一行不需要换行
                        if (postStream.Length == 0)
                            formdataBytes = Encoding.UTF8.GetBytes(formdata.Substring(2, formdata.Length - 2));
                        else
                            formdataBytes = Encoding.UTF8.GetBytes(formdata);
                        postStream.Write(formdataBytes, 0, formdataBytes.Length);

                    }
                    //结尾
                    var footer = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
                    postStream.Write(footer, 0, footer.Length);

                }
                else
                {
                    request.ContentType = "application/x-www-form-urlencoded";
                }
                #endregion

                request.ContentLength = postStream.Length;

                #region 输入二进制流
                if (postStream != null)
                {
                    postStream.Position = 0;
                    //直接写入流
                    Stream requestStream = request.GetRequestStream();

                    byte[] buffer = new byte[1024];
                    int bytesRead = 0;
                    while ((bytesRead = postStream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        requestStream.Write(buffer, 0, bytesRead);
                    }

                    //postStream.Seek(0, SeekOrigin.Begin);
                    //StreamReader sr = new StreamReader(postStream);
                    //var postStr = sr.ReadToEnd();
                    postStream.Close();//关闭文件访问
                }
                #endregion
                HttpWebResponse response = null;

                response = (HttpWebResponse)request.GetResponse();


                if (cookieContainer != null)
                {
                    response.Cookies = cookieContainer.GetCookies(response.ResponseUri);
                }

                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader myStreamReader = new StreamReader(responseStream, encoding ?? Encoding.UTF8))
                    {
                        string retString = myStreamReader.ReadToEnd();
                        return retString;
                    }
                }
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
        }

    }
}

