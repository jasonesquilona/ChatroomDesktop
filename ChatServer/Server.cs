using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using ChatroomDesktop.Models;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using SqlConnection = Microsoft.Data.SqlClient.SqlConnection;

namespace ChatServer;

public class Server
{
    
    private TcpListener _tcpListener;
    private TcpListener _tcpListenerSignup;
    private List<Client> _clients = new List<Client>();

    private  SqlConnection _sqlConnection;
    
    private object _clientsLock = new object();
    public Server(IPAddress ip)
    {
        var ipEndPoint = new IPEndPoint(ip, 8080);
        _tcpListener = new TcpListener(ipEndPoint);
        var signUpEndPoint = new IPEndPoint(ip, 8081);
        _tcpListenerSignup = new TcpListener(signUpEndPoint);
        string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=master;Integrated Security=True;";
        _sqlConnection = new SqlConnection(connectionString);
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
        var jsonString = Encoding.UTF8.GetString(buffer, 0, bytesRead);
        Message message = JsonSerializer.Deserialize<Message>(jsonString);
        if (message?.MessageType == "SIGNUP")
        {
            var name = message.Sender;
            var password = message.ChatMessage;
            Console.WriteLine($"{name}: {password}");
            var response = "201 User registered";
            byte[] messageBytes = Encoding.UTF8.GetBytes(response);
            await stream.WriteAsync(messageBytes, 0, messageBytes.Length);
            bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            jsonString = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            message = JsonSerializer.Deserialize<Message>(jsonString);
        }
        
        if (message?.MessageType == "JOIN")
        {
            var name = message.Sender;
            var client = new Client(clientID, name);
            lock (_clientsLock)
            {
                _clients.Add(client);
                Console.WriteLine($"Client connected: {clientID.Client.RemoteEndPoint}, Name: {name}");
                _ = HandleClientAsync(client);
                List<string> currClients = new List<string>();
                foreach (var clientName in _clients)
                {
                    currClients.Add(clientName.Name);
                }
                message.UserList = currClients.ToArray();
                BroadcastMessage(message);
            }
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
                if (bytesRead == 0)
                {
                    Console.WriteLine($"{client.Name} disconnected");
                    break;
                }
                
                var message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Client {client.ClientID.Client.RemoteEndPoint}| {client.Name} sent message: {message}");
                var messageObj = new Message {MessageType = "CHAT",Sender  = client.Name, ChatMessage = message, UserList = GetUserList()};
                await BroadcastMessage(messageObj);
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

    private async Task BroadcastMessage(Message message)
    {
        if (message.MessageType == "JOIN")
        {
            message.ChatMessage = $"{message.Sender} has joined!";
            
        }
        else if (message.MessageType == "CHAT")
        {
            message.ChatMessage = $"{message.Sender}: {message.ChatMessage}";
        }
        string jsonString = JsonSerializer.Serialize(message);
        byte[] messageBytes = Encoding.UTF8.GetBytes(jsonString);
        foreach (var client in _clients)
        {
            var clientId = client.ClientID;
            var name = client.Name;
            NetworkStream stream = clientId.GetStream();
            Console.WriteLine($"Sending message: {message.ChatMessage} to {clientId.Client.RemoteEndPoint} | {name}");
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
        var messageObj = new Message {MessageType = "DISCONNECT",Sender  = client.Name, ChatMessage = $"{client.Name} has left", UserList = GetUserList()};
        BroadcastMessage(messageObj);
    }

    private string[] GetUserList()
    {
        List<string> currClients = new List<string>();
        foreach (var clientName in _clients)
        {
            currClients.Add(clientName.Name);
        }
        
        return currClients.ToArray();
    }
}