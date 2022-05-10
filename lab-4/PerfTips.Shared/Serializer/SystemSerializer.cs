using Utf8Json;

namespace PerfTips.Shared.Serializer;

public class SystemSerializer : ISerializer
{
    public byte[] Serialize<T>(T obj) => JsonSerializer.Serialize(obj);

    public T Deserialize<T>(byte[] bytes) => JsonSerializer.Deserialize<T>(bytes);
}