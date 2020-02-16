using System;
using System.Collections;
using System.Collections.Generic;
using redolib.Serializers;

namespace redolib.DataTypes
{
    class RedisList<T> : IRedisCollection<T>
    {
        public StackExchange.Redis.RedisKey Key { get; }
        public IRedisConnection Connection { get; }
        public IRedisDataSerializer<T> Serializer { get; }

        internal RedisList (IRedisConnection con, string key, IRedisDataSerializer<T> ser)
        {
            Key = key;
            Connection = con;
            Serializer = ser;
        }

        public IEnumerator<T> GetEnumerator ()
        {
            foreach (var item in Connection.Database.ListRange (Key))
                yield return Serializer.DeSerialize (item);
        }
        IEnumerator IEnumerable.GetEnumerator () =>
        throw new NotImplementedException ();

        public long Add (object rval)
        {
            var _serializedObj = Serializer.Serialize (rval);
            return Connection.Database.ListRightPush (Key, _serializedObj);
        }
        public long Remove (object rval)
        {
            var _serializedObj = Serializer.Serialize (rval);
            return Connection.Database.ListRemove (Key, _serializedObj);
        }
    }
}