using System.Collections.Generic;
using System.Linq;

namespace redo_mq
{
    public class RedisContext
    {
        public IRedisConnection Connection { get; }

        public RedisContext (IRedisConnection rc) { Connection = rc; }
        public RedisContext ()
        {
            Connection = new RedisConnection ("127.0.0.1:6379");
        }

        List<IRedisCollection> _collections = new List<IRedisCollection> ();

        internal IRedisCollection Register<T> (IRedisCollection r_l)
        {
            _collections.Add (r_l);
            return r_l;
        }

        internal IEnumerable<string> GetCollection<T> (string key) where T : IRedisCollection =>
            _collections.OfType<T> ().SingleOrDefault (i => i.Key == key);
    }
}