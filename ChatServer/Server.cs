using System.Net;
using System.Net.Sockets;
using System.Text;


namespace ChatServer;

public class Server
{
    
    private TcpListener _tcpListener;
    private List<Client> _clients = new List<Client>();
    
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
            var clientID = await _tcpListener.AcceptTcpClientAsync();
            _ = HandleNewConnection(clientID);
        }
    }

    private async Task HandleNewConnection(TcpClient clientID)
    {
        NetworkStream stream = clientID.GetStream();
        var buffer = new byte[1024];
        var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
        var name = Encoding.UTF8.GetString(buffer, 0, bytesRead);
        var client = new Client(clientID, name);
        lock (_clientsLock)
        {
            _clients.Add(client);
            Console.WriteLine($"Client connected: {clientID.Client.RemoteEndPoint}, Name: {name}");
            _ = HandleClientAsync(client);
        }
    }

    private async Task HandleClientAsync(Client client)
    {
        var buffer = new byte[1024];
        NetworkStream stream = client.ClientID.GetStream();
    
        try
        {
            while (true)
            {
                var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                if (bytesRead == 0) break;
                
                var message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Client {client.ClientID.Client.RemoteEndPoint}| {client.Name} sent message: {message}");
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

    private async Task BroadcastMessage(string message, Client sender)
    {

        message = $"{sender.Name} : {message}";
        byte[] messageBytes = Encoding.UTF8.GetBytes(message);
        foreach (var client in _clients)
        {
            var clientId = client.ClientID;
            var name = client.Name;
            if (clientId == sender.ClientID)
            {
                Console.WriteLine($"Same Sender");
                continue;
            }
            NetworkStream stream = clientId.GetStream();
            Console.WriteLine($"Sending message: {message} to {clientId.Client.RemoteEndPoint} | {name}");
            await stream.WriteAsync(messageBytes, 0, messageBytes.Length);
        }
    }

    public void DisconnectUser(Client client)
    {
        lock (_clientsLock)
        {
            _clients.Remove(client);
            client.ClientID.Close();
        }
    }
}