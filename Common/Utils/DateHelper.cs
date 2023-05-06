using System;

namespace Common.Utils
{
    public class DateHelper
    {
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static string GetTimeStamp()
        {
            //var ts = DateTime.Now.ToString("yyyy-MM-dd");
            //ts = "A" + ts.Replace("-", "").Replace(" ", "").Replace(":", "");
            //return ts;
            TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
    }
}
