using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using ChatroomDesktop.Models;
using Microsoft.Data.SqlClient;
using SqlConnection = Microsoft.Data.SqlClient.SqlConnection;

namespace ChatServer;

public class Server
{
    
    private TcpListener _tcpListener;
    private List<Client> _clients = new List<Client>();
    
    private object _clientsLock = new object();
    
    private Lock _databasesLock = new Lock();
    public Server(IPAddress ip)
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
            Console.WriteLine($"Client connected: {clientID.Client.RemoteEndPoint}");
            _ = HandleNewConnection(clientID);
        }
    }
    
    private async Task HandleNewConnection(TcpClient clientID)
    {
        NetworkStream stream = clientID.GetStream();
        var buffer = new byte[1024];
        Client client = null;
        try
        {
            while (true)
            {
                var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                if (bytesRead == 0)
                {
                    Console.Write($"Client disconnected: {clientID.Client.RemoteEndPoint}");
                    break;
                }
                Console.WriteLine("Message received");
                var jsonString = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Message received: {jsonString}");
                Message? message = JsonSerializer.Deserialize<Message>(jsonString);
                if (message?.MessageType == "SIGNUP")
                {
                    Console.WriteLine("Sign up request received");

                    var sql = "INSERT INTO ChatSchema.Users (Username, Password) VALUES  (@username, @password)";

                    if (!SendSQL(sql, message))
                    {
                        Console.WriteLine("Sign up failed");
                    }

                    var response = "201 User registered";
                    byte[] messageBytes = Encoding.UTF8.GetBytes(response);
                }
                else if (message?.MessageType == "JOIN")
                {
                    var name = message.Sender;
                    client = new Client(clientID, name);
                    lock (_clientsLock)
                    {
                        _clients.Add(client);
                        Console.WriteLine($"Client connected: {clientID.Client.RemoteEndPoint}, Name: {name}");
                        //_ = HandleClientAsync(client);
                        List<string> currClients = new List<string>();
                        foreach (var clientName in _clients)
                        {
                            currClients.Add(clientName.Name);
                        }

                        message.UserList = currClients.ToArray();
                        BroadcastMessage(message);
                    }
                }
                else if (message?.MessageType == "LOGIN")
                {
                    var sql = "SELECT * FROM ChatSchema.Users WHERE Username = @username AND Password = @password";
                    if (!SendSQL(sql, message))
                    {
                        Console.WriteLine("Login failed");
                    }
                    else
                    {

                    }

                }
                else if (message?.MessageType == "CHAT")
                {
                    if(client != null){
                        var messageObj = new Message {MessageType = "CHAT",Sender  = message.Sender, ChatMessage = message.ChatMessage, UserList = GetUserList()};
                        await BroadcastMessage(messageObj);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            //DisconnectUser(clientID.Client);
        }
    }

    /*private async Task HandleClientAsync(Client client)
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
    }*/

    private async Task BroadcastMessage(Message? message)
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

    private bool SendSQL(string sql, Message? message)
    {
        var name = message.Sender;
        var password = message.ChatMessage;
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;";
        int rowsAffected;
        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        {
            Console.WriteLine($"Sending New User to Database");
            try
            {
                sqlConnection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine($"Connected to Database");
            using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
            {
                Console.WriteLine("New Command");
                sqlCommand.Parameters.Add("@username", System.Data.SqlDbType.VarChar).Value = name;
                sqlCommand.Parameters.Add("@password", System.Data.SqlDbType.VarChar).Value = password;
                Console.WriteLine("Executing Command");
                rowsAffected = sqlCommand.ExecuteNonQuery();
                Console.WriteLine($"rows affected: {rowsAffected}");
            }
        }
        if (rowsAffected == 0)
        {
            Console.WriteLine("No rows affected");
            return false;
        }
        else
        {
            return true;
        }
    }
}