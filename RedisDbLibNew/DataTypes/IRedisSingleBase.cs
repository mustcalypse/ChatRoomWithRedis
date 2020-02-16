using redolib.Serializers;
using StackExchange.Redis;

namespace redolib.DataTypes
{
    interface IRedisSingleBase<out T>
    {
        IRedisConnection Connection { get; }
        IRedisDataSerializer<T> Serializer { get; }
        bool Add (RedisKey Key, object value);
        bool Remove (RedisKey Key, object value);
    }
}