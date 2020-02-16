using redolib.Serializers;
using StackExchange.Redis;

namespace redolib.DataTypes
{
    class RedisString<T> : IRedisSingleBase<T>
    {
        public IRedisConnection Connection { get; }
        public IRedisDataSerializer<T> Serializer { get; }
        internal RedisString (IRedisConnection con, IRedisDataSerializer<T> ser)
        {
            Connection = con;
            Serializer = ser;
        }
        public bool Add (RedisKey Key, object value)
        {
            return Connection.Database.StringSet (Key, Serializer.Serialize (value));
        }

        public bool Remove (RedisKey Key, object value)
        {
            return Connection.Database.StringSet (Key, StackExchange.Redis.RedisValue.Null);
        }
    }
}