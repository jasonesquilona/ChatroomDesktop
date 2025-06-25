using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using ChatroomDesktop.Models;
using ChatroomDesktop.Utilities;

namespace ChatServer;

public class Server
{
    
    private TcpListener _tcpListener;
    private List<Client> _loggedInClients = new List<Client>();
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
                    if (chatMsg.ChatType == "JOIN")
                    {
                        //client = await HandleJoinClient(clientID, chatMsg);
                    }
                    else if (chatMsg.ChatType == "CHAT")
                    {
                        await HandleChatMessage(client, chatMsg);
                    }
                }
                else if (message is LoginMessage loginMsg)
                {
                    bool result = await HandleLogin(loginMsg, stream);
                    if (result == true)
                    {
                        client = new Client(clientID, loginMsg.Username);
                        lock(_clientsLock){
                            _loggedInClients.Add(client);
                        }
                        Console.WriteLine($"Client logged in: {client.Name}");
                    }
                }
                else if (message is CreateGroupMessage createGroupMsg)
                {
                    await HandleCreateGroup(createGroupMsg, stream);
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
            var messageObj = new ChatMessage {ChatType = "CHAT", Sender = message.Sender, chatMessage = message.chatMessage, UserList = GetUserList()};
            await BroadcastMessage(messageObj);
        }
    }

    private async Task<bool> HandleLogin(LoginMessage message, NetworkStream stream)
    {
        var sql = "SELECT password FROM ChatSchema.Users WHERE username = @username";
        string response = "";
        (string password, bool success) = SQLOperations.SendSQLLogin(sql, message);
        if (!success)
        {
            Console.WriteLine("Login failed");
            response = "401 Unauthorized";
        }
        else
        {
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
        return success;
    }

    private async Task<Client> HandleConnectClient(TcpClient clientID, ConnectMessage message)
    {
        var name = message.User;
        var client = new Client(clientID, name);
        lock (_clientsLock)
        {
            _loggedInClients.Add(client);
            Console.WriteLine($"Client connected: {clientID.Client.RemoteEndPoint}, Name: {name}");
        }
        return client;
    }
    
    private async Task HandleSignup(SignupMessage message, NetworkStream stream)
    {
        Console.WriteLine("Sign up request received");

        var sql = "INSERT INTO ChatSchema.Users (Username, Password) VALUES  (@username, @password)";
        string response = "";
        if (!SQLOperations.SendSQLSignup(sql, message))
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
    
    private async Task<bool> HandleCreateGroup(CreateGroupMessage createGroupMsg, NetworkStream stream)
    {
        var groupName = createGroupMsg.groupName;
        bool response = false;
        var sql = "INSERT INTO ChatSchema.Groups (Name, Code)  VALUES (@Name, @Code)";
        for (int i = 0; i < 5; i++)
        {
            var groupCode = Util.GenrateRandomString();
            response = SQLOperations.SendNewGroup(sql, groupName, groupCode);
            if (response)
            {
                break;
            }
        }

        var responseMessage = "";
        if (response)
        {
            
        }
        else
        {
            responseMessage = "400 Bad Request";
        }
        byte[] messageBytes = Encoding.UTF8.GetBytes(responseMessage);
        await stream.WriteAsync(messageBytes, 0, messageBytes.Length);
        return response;
    }


    //Broadcast everyone to currently connected Chat
    private async Task BroadcastMessage(ChatMessage? message)
    {
        if (message.ChatType == "JOIN")
        {
            message.chatMessage = $"{message.Sender} has joined!";
            
        }
        else if (message.ChatType == "CHAT")
        {
            message.chatMessage = $"{message.Sender}: {message.chatMessage}";
        }
        string jsonString = JsonSerializer.Serialize(message);
        byte[] messageBytes = Encoding.UTF8.GetBytes(jsonString);
        lock (_clientsLock)
        {
            foreach (var client in _loggedInClients)
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
            _loggedInClients.RemoveAll(p => p.ClientID == client);
            Console.WriteLine($"Client disconnected from {client.Client.RemoteEndPoint}");
            client.Close();
        }
    }

    private string[] GetUserList()
    {
        List<string> currClients = new List<string>();
        foreach (var clientName in _loggedInClients)
        {
            currClients.Add(clientName.Name);
        }
        
        return currClients.ToArray();
    }
    
}