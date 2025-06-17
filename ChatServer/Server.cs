using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using ChatroomDesktop.Models;
using ChatroomDesktop.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic.CompilerServices;
using SqlConnection = Microsoft.Data.SqlClient.SqlConnection;

namespace ChatServer;

public class Server
{
    
    private TcpListener _tcpListener;
    private List<Client> _clients = new List<Client>();
    private List<TcpClient> _tcpClients = new List<TcpClient>();
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
        _tcpClients.Add(clientID);
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
                var jsonString = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Message received: {jsonString}");
                JsonSerializerOptions options =new() { AllowOutOfOrderMetadataProperties = true };
                Message? message = JsonSerializer.Deserialize<Message>(jsonString,options);
                if (message is SignupMessage signupMsg)
                {
                    await HandleSignup(signupMsg, stream);
                }
                else if (message is ChatMessage chatMsg)
                {
                    if (chatMsg.MessageType == "JOIN")
                    {
                        client = await HandleJoinClient(clientID, chatMsg);
                    }
                    else if (chatMsg.MessageType == "CHAT")
                    {
                        await HandleChatMessage(client, chatMsg);
                    }
                    else if (chatMsg.MessageType == "DISCONNECT")
                    {
                    }
                }
                else if (message is LoginMessage loginMsg)
                {
                    await HandleLogin(loginMsg, stream);
                    _clients.Add(new Client(clientID, loginMsg.Username));
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            DisconnectClient(clientID);
        }
    }

    private async Task HandleChatMessage(Client? client, ChatMessage message)
    {
        if(client != null){
            var messageObj = new ChatMessage {MessageType = "CHAT", ChatType = "CHAT", Sender = message.Sender, chatMessage = message.chatMessage, UserList = GetUserList()};
            await BroadcastMessage(messageObj);
        }
    }

    private async Task HandleLogin(LoginMessage message, NetworkStream stream)
    {
        var sql = "SELECT password FROM ChatSchema.Users WHERE username = @username";
        string response = "";
        (string password, bool success) = SendSQLLogin(sql, message);
        if (!success)
        {
            Console.WriteLine("Login failed");
            response = "401 Unauthorized";
        }
        else
        {
            Console.WriteLine("Checking password");
            bool isCorrect = Util.CheckPassword(message.Password,password);
            if (isCorrect)
            {
                response = "201 User Login";
            }
            else
            {
                response = "400 Bad Request";
            }
        }
                    
        byte[] messageBytes = Encoding.UTF8.GetBytes(response);
        await stream.WriteAsync(messageBytes, 0, messageBytes.Length);
    }

    private async Task<Client> HandleJoinClient(TcpClient clientID, ChatMessage message)
    {
        var name = message.Sender;
        var client = new Client(clientID, name);
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
        }
        await BroadcastMessage(message);
        return client;
    }

    private async Task HandleSignup(SignupMessage message, NetworkStream stream)
    {
        Console.WriteLine("Sign up request received");

        var sql = "INSERT INTO ChatSchema.Users (Username, Password) VALUES  (@username, @password)";
        string response = "";
        if (!SendSQLSignup(sql, message))
        {
            Console.WriteLine("Sign up failed");
            response = "401 Unauthorized";
        }
        else
        {
            response = "201 User registered";
        }
                    
        byte[] messageBytes = Encoding.UTF8.GetBytes(response);
        await stream.WriteAsync(messageBytes, 0, messageBytes.Length);
    }

    private async Task BroadcastMessage(ChatMessage? message)
    {
        if (message.ChatType == "JOIN")
        {
            message.chatMessage = $"{message.Sender} has joined!";
            
        }
        else if (message.MessageType == "CHAT")
        {
            message.chatMessage = $"{message.Sender}: {message.chatMessage}";
        }
        string jsonString = JsonSerializer.Serialize(message);
        byte[] messageBytes = Encoding.UTF8.GetBytes(jsonString);
        lock (_clientsLock)
        {
            foreach (var client in _clients)
            {
                var clientId = client.ClientID;
                var name = client.Name;
                NetworkStream stream = clientId.GetStream();
                Console.WriteLine($"Sending message: {message.chatMessage} to {clientId.Client.RemoteEndPoint} | {name}");
                stream.Write(messageBytes, 0, messageBytes.Length);
            }
        }
    }

    public void DisconnectClient(TcpClient client)
    {
        lock (_clientsLock)
        {
            _tcpClients.Remove(client);
            client.Close();
        }
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

    private bool SendSQLSignup(string sql, SignupMessage? message)
    {
        var name = message.Username;
        var password = message.Password;
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
            }
        }
        Console.WriteLine($"rows affected: {rowsAffected}");
        if (rowsAffected <= 0)
        {
            Console.WriteLine("No rows affected");
            return false;
        }
        else
        {
            
            return true;
        }
    }

    private (string, bool) SendSQLLogin(string sql, LoginMessage? message)
    {
        var name = message.Username;
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;";
        object result;
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
                Console.WriteLine("Executing Command");
                result = sqlCommand.ExecuteScalar();
            }
        }
        if (result == null)
        {
            Console.WriteLine("No rows affected");
            return ("", false);
        }
        else
        {
            string retrievedPassword = (string)result;
            Console.WriteLine(retrievedPassword);
            return (retrievedPassword, true);
        }
    }
}