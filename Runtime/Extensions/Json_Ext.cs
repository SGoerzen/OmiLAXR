using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace OmiLAXR.Extensions
{
    public static class Json_Ext
    {
        public static Dictionary<string, object> Flatten(this JToken token, string prefix = "")
        {
            var result = new Dictionary<string, object>();

            if (token is JObject obj)
            {
                foreach (var prop in obj.Properties())
                {
                    var nested = Flatten(prop.Value, $"{prefix}{prop.Name}_");
                    foreach (var kvp in nested)
                        result[kvp.Key] = kvp.Value;
                }
            }
            else if (token is JArray array)
            {
                for (var i = 0; i < array.Count; i++)
                {
                    var nested = Flatten(array[i], $"{prefix}{i}_");
                    foreach (var kvp in nested)
                        result[kvp.Key] = kvp.Value;
                }
            }
            else if (token is JValue val)
            {
                result[prefix.TrimEnd('_')] = val.Value;
            }

            return result;
        }
    }
}