using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace redo_mq
{
    public abstract class RedisHelper<T>
    {
        protected RedisContext _stor;
        protected string _key;
        private IRedisCollection _coll;
        public RedisHelper (RedisContext st, string key)
        {
            _stor = st;
            _key = key;
            _coll = _stor.Register<T> (new RedisList (_stor.Connection, _key));
        }

        public IEnumerable<T> Get () => _coll.Select (i => JsonConvert.DeserializeObject<T> (i));

        protected void Publish (string _json_string, string mq_key) =>
        _stor.Connection.Subscriber.Publish (mq_key, _json_string);

        protected void Subscribe<T1> (string mq_key, System.Action<T1> value)
        {
            StackExchange.Redis.ChannelMessageQueue cmq = _stor.Connection.Subscriber.Subscribe (mq_key);
            cmq.OnMessage (i => value?.Invoke (JsonConvert.DeserializeObject<T1> (i.Message)));
        }
        protected void Unsubscribe (string mq_key)
        {
            _stor.Connection.Subscriber.Unsubscribe (mq_key);
        }

        protected TResult Execute<Tobj, TResult> (Tobj ob, System.Func<string, TResult> act, string _key)
        {
            var _s = JsonConvert.SerializeObject (ob);
            Publish (_s, _key);
            if (act != null) return act.Invoke (_s);
            return default (TResult);
        }

        public long Add (T o) => Execute (o, _coll.Add, MqNameAdded ());
        string MqNameAdded () => $"{_key}_added";
        public event System.Action<T> Added
        {
            add { Subscribe<T> (MqNameAdded (), value); }
            remove { Unsubscribe (MqNameAdded ()); }
        }

        public long Remove (T o) => Execute (o, _coll.Remove, MqNameRemoved ());
        string MqNameRemoved () => $" {_key}_removed";
        public event System.Action<T> Removed
        {
            add { Subscribe<T> (MqNameRemoved (), value); }
            remove { Unsubscribe (MqNameRemoved ()); }
        }
    }
}