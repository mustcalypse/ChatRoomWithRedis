using StackExchange.Redis;

namespace redolib
{
    public interface IRedisConnection
    {
        IDatabase Database { get; }
    }

    public class DefaultRedisConnection : IRedisConnection
    {
        public IDatabase Database { get; }
        public DefaultRedisConnection (string redis_conf = "127.0.0.1:6379")
        {
            Database = ConnectionMultiplexer.Connect (redis_conf).GetDatabase (0);
        }
    }
}