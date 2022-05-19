using Microsoft.IO;

namespace PerfTips.Shared.Serializer;

public class ProtoBufSerializer : ISerializer
{
    private static readonly RecyclableMemoryStreamManager Manager = new();

    public byte[] Serialize<T>(T obj)
    {
        using var memoryStream = Manager.GetStream() as RecyclableMemoryStream;
        
        ProtoBuf.Serializer.Serialize(memoryStream, obj);
        return memoryStream.GetBuffer();
    }

    public T Deserialize<T>(byte[] bytes)
    {
        using var memoryStream = Manager.GetStream(bytes) as RecyclableMemoryStream;

        return ProtoBuf.Serializer.Deserialize<T>(memoryStream);
    }
}