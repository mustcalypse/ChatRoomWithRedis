using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using StackExchange.Redis;

namespace redolib.Serializers
{
    sealed class ByteArraySerializer<T> : IRedisDataSerializer<T>
    {
        BinaryFormatter bFormatter = new BinaryFormatter ();
        public T DeSerialize (RedisValue serializedValue)
        {
            using (var ms = new System.IO.MemoryStream ((byte[]) serializedValue))
            {
                return (T) bFormatter.Deserialize (ms);
            }
        }

        public RedisValue Serialize (object obj)
        {
            using (var ms = new System.IO.MemoryStream ())
            {
                bFormatter.Serialize (ms, obj);
                return RedisValue.CreateFrom (ms);
            }
        }
    }
}