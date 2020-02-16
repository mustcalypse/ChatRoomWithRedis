using redolib.Serializers;
using StackExchange.Redis;

namespace redolib.DataTypes
{
    class DataEntrySerializer
    {
        public RedisValue DeSerialize (RedisValue serializedValue)
        {

        }

        public HashEntry[] Serialize (object obj)
        {
            var p = obj.GetType ().GetProperties ();
            int len = p.Length;
            var h_e_s = new HashEntry[len];
            for (int i = 0; i < len; i++)
                h_e_s[i] = new HashEntry (p[i].Name, p[i].GetValue (obj).ToString ());
            return h_e_s;
        }
    }

    class RedisHash<T> : DataTypes.IRedisSingleBase<T>
    {
        public IRedisConnection Connection { get; }
        public DataEntrySerializer Serializer { get; }

        IRedisDataSerializer<T> IRedisSingleBase<T>.Serializer =>
        throw new System.NotImplementedException ();

        public RedisHash (IRedisConnection conn)
        {
            Connection = conn;
            Serializer = new DataEntrySerializer ();
        }

        public bool Add (RedisKey Key, object value)
        {
            Connection.Database.HashSet (Key, Serializer.Serialize (value));
            return true;
        }

        public bool Remove (RedisKey Key, object value)
        {
             return Connection.Database.HashDelete (Key, Serializer.Serialize (value));
        }
    }
}