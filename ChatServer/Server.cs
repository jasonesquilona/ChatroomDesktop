using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatServer;

public class Server
{
    
    private TcpListener _tcpListener;
    private List<TcpClient> _clients = new List<TcpClient>();
    
    private object _clientsLock = new object();
    public Server(IPAddress ip, int port)
    {
        var ipEndPoint = new IPEndPoint(ip, 8080);
        _tcpListener = new TcpListener(ipEndPoint);
    }

    public async Task ListenForClients()
    {
        _tcpListener.Start();
        Console.WriteLine("Listening for connections");
        while (true)
        {
            var client = await _tcpListener.AcceptTcpClientAsync();
            lock (_clientsLock)
            {
                _clients.Add(client);
                Console.WriteLine($"Client connected: {client.Client.RemoteEndPoint}");
                _ = HandleClientAsync(client);
            }
        }
    }

    private async Task HandleClientAsync(TcpClient client)
    {
        var buffer = new byte[1024];
        NetworkStream stream = client.GetStream();

        try
        {
            while (true)
            {
                var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                if (bytesRead == 0) break;
                
                var message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Client {client.Client.RemoteEndPoint} sent message: {message}");
                await BroadcastMessage(message, client);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
          DisconnectUser(client);
        }
    }

    private async Task BroadcastMessage(string message, TcpClient sender)
    {
        byte[] messageBytes = Encoding.UTF8.GetBytes(message);

        foreach (var client in _clients)
        {
            if (client == sender)
            {
                Console.WriteLine($"Same Sender");
                continue;
            }
            NetworkStream stream = client.GetStream();
            Console.WriteLine($"Sending message: {message} to {client.Client.RemoteEndPoint}");
            await stream.WriteAsync(messageBytes, 0, messageBytes.Length);
        }
    }

    public void DisconnectUser(TcpClient client)
    {
        lock (_clientsLock)
        {
            _clients.Remove(client);
            client.Close();
        }
    }
}