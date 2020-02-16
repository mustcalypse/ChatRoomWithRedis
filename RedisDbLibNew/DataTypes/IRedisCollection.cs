using System.Collections.Generic;

namespace redolib.DataTypes
{
    interface IRedisCollection<out T> : IRedisContainer<T>, IEnumerable<T>
    {
        StackExchange.Redis.RedisKey Key { get; }
        long Add (object value);
        long Remove (object value);
    }
}