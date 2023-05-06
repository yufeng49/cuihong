using Newtonsoft.Json;
using System.Collections.Generic;

namespace Common.Utils
{
    public static class JsonHelper
    {
        public static string SerializeObject(object value)
        {
            return JsonConvert.SerializeObject(value);
        }

        public static T DeserializeObject<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        public static string SerializeDictionaryToJsonString<TKey, TValue>(Dictionary<TKey, TValue> dic)
        {
            return JsonConvert.SerializeObject(dic);
        }

        public static Dictionary<TKey, TValue> DeserializeStringToDictionary<TKey, TValue>(string jsonStr)
        {
            if (string.IsNullOrEmpty(jsonStr))
                return new Dictionary<TKey, TValue>();

            Dictionary<TKey, TValue> jsonDict = JsonConvert.DeserializeObject<Dictionary<TKey, TValue>>(jsonStr);

            return jsonDict;

        }

    }

}
