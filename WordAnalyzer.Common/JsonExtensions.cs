using Newtonsoft.Json;

namespace WordAnalyzer.Common
{
    public static class JsonExtensions
    {
        public static string ToJson(this object item)
        {
            return item == null ? "{}" : JsonConvert.SerializeObject(item);
        }
    }
}
