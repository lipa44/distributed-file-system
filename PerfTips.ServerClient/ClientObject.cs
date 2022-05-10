using System.Net.Sockets;
using System.Text;

namespace PerfTips.ServerClient;

public class ClientObject
{
    private readonly TcpClient _client;

    public ClientObject(TcpClient tcpClient) =>
        _client = tcpClient ?? throw new ArgumentNullException(nameof(tcpClient));

    public void Process()
    {
        NetworkStream? networkStream = default;
        try
        {
            networkStream = _client.GetStream();
            var data = new byte[64];
            while (true)
            {
                var builder = new StringBuilder();
                do
                {
                    var bytes = networkStream.Read(data, 0, data.Length);
                    builder.Append(Encoding.UTF8.GetString(data, 0, bytes));
                } while (networkStream.DataAvailable);

                var message = builder.ToString();

                Console.WriteLine(message);

                var answer = message[(message.IndexOf(':') + 1)..].Trim().ToUpper();
                data = Encoding.Unicode.GetBytes(answer);
                networkStream.Write(data, 0, data.Length);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            networkStream?.Close();
            _client.Close();
        }
    }
}