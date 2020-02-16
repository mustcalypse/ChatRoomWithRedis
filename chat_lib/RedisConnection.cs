using StackExchange.Redis;

namespace redo_mq
{
    public interface IRedisConnection
    {
        ISubscriber Subscriber { get; }
        IDatabase Database { get; }
    }

    public class RedisConnection : IRedisConnection
    {
        public ISubscriber Subscriber { get; }
        public IDatabase Database { get; }
        public RedisConnection (string redis_conf)
        {
            Database = ConnectionMultiplexer.Connect (redis_conf).GetDatabase (0);
            Subscriber = Database.Multiplexer.GetSubscriber ();
        }
    }
}