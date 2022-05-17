namespace PerfTips.Shared.Serializer;

public class ProtoBufSerializer : ISerializer
{
    public byte[] Serialize<T>(T obj)
    {
        using var memoryStream = new MemoryStream();
        
        ProtoBuf.Serializer.Serialize(memoryStream, obj);
        return memoryStream.ToArray();
    }

    public T Deserialize<T>(byte[] bytes)
    {
        using var memoryStream = new MemoryStream(bytes);

        return ProtoBuf.Serializer.Deserialize<T>(memoryStream);
    }
}