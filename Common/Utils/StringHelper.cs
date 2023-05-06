using System;

namespace Common.Utils
{
    public static class StringHelper
    {
        public static string ToString(this DateTime time, string str, int num)
        {
            if (str == "yy-mm-dd")
            {
                return time.ToString("yy-MM-dd").Replace("-", "");
            }
            if (str == "MM")
            {
                return Int32.Parse(time.ToString("MM")).ToString();
            }
            return time.ToString();
        }
    }
}
