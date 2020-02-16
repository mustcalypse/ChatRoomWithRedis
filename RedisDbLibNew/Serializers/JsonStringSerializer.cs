using Newtonsoft.Json;
using StackExchange.Redis;

namespace redolib.Serializers
{
    sealed class JsonStringSerializer<T> : IRedisDataSerializer<T>
    {
        public T DeSerialize (RedisValue serializedValue) =>
        JsonConvert.DeserializeObject<T> (serializedValue);
        public RedisValue Serialize (object obj) =>
        JsonConvert.SerializeObject (obj);
    }
}