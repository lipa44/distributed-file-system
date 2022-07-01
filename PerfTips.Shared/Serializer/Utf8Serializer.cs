namespace PerfTips.Shared.Serializer;

public class Utf8Serializer : ISerializer
{
    public byte[] Serialize<T>(T obj) => Utf8Json.JsonSerializer.Serialize(obj);

    public T Deserialize<T>(byte[] bytes) => Utf8Json.JsonSerializer.Deserialize<T>(bytes);
}