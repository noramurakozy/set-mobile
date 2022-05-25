using Newtonsoft.Json;

namespace Statistics
{
    public static class JsonUtils
    {
        public static readonly JsonSerializerSettings SerializerSettings = new()
        {
            TypeNameHandling = TypeNameHandling.All
        };
    }
}