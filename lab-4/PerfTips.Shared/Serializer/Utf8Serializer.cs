using Microsoft.IO;

namespace PerfTips.Shared.Serializer;

public class Utf8Serializer : ISerializer
{
    private static readonly RecyclableMemoryStreamManager Manager = new();

    public byte[] Serialize<T>(T obj)
    {
        return Utf8Json.JsonSerializer.Serialize(obj);
    }

    public T Deserialize<T>(byte[] bytes)
    {
        return Utf8Json.JsonSerializer.Deserialize<T>(bytes);
    }
}