namespace Common.Utils
{
    public static class ConfigHelper
    {
        private static string xmlPath = System.AppDomain.CurrentDomain.BaseDirectory + "ConfigInfo.xml";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="initialValue">初始值</param>
        /// <returns></returns>
        public static string GetValue(string key, string initialValue)
        {
            var serverurl = XMLHelper.GetXmlNodeValueByXpath(xmlPath, "Config", key, initialValue);///Config/
            return serverurl;
            //Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //return config.AppSettings.Settings[key].Value;
        }

        public static void Modify(string key, string value)
        {
            XMLHelper.CreateOrUpdateXmlNodeByXPath(xmlPath, "/Config/" + key, value);
            //Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //config.AppSettings.Settings[key].Value = value;
            ////保存
            //config.Save();
        }

    }
}
