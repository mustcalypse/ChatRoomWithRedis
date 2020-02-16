using redolib.Serializers;

namespace redolib.DataTypes
{
    interface IRedisContainer<out T>
    {
        IRedisConnection Connection { get; }
        IRedisDataSerializer<T> Serializer { get; }
    }
}