using System.Collections.Generic;
using System.Linq;
using redolib.DataTypes;
using redolib.Serializers;

namespace redolib
{
    public abstract class RedisSingle<T>
    {
        RedisString<T> _single;

        private RedisSingle (IRedisConnection conn, bool jsonSer = false)
        {
            _single = new RedisString<T> (conn, new JsonStringSerializer<T> ());
        }
    }

    //serializers speed comparison |>  binary => T(get)=35ms, T(add)=18ms  ;  true => T(get)=185ms, T(add)=155ms
    public abstract class RedisHelper<T>
    {
        IRedisCollection<T> _coll;

        public RedisHelper (IRedisConnection conn, string key, bool jsonSer = false)
        {
            _coll = new RedisList<T> (conn, key,
                jsonSer?(IRedisDataSerializer<T>) new JsonStringSerializer<T> ():
                (IRedisDataSerializer<T>) new ByteArraySerializer<T> ());
        }

        public IEnumerable<T> Get () => _coll;
        public long Add (T ob) => _coll.Add (ob);
        public long Remove (T ob) => _coll.Remove (ob);
    }
}