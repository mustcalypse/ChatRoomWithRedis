using Newtonsoft.Json;
using StackExchange.Redis;

namespace redolib.Serializers
{
    interface IRedisDataSerializer<out T>
    {
        RedisValue Serialize (object obj);
        T DeSerialize (RedisValue serializedValue);
    }
}