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
    private List<Client?> _loggedInClients = new List<Client?>();
    private List<TcpClient> _tcpClients = new List<TcpClient>();
    private object _clientsLock = new object();
    private SQLOperations _sqlOperations;
    
    private Lock _databasesLock = new Lock();
    public Server(IPAddress ip)
    {
        var ipEndPoint = new IPEndPoint(ip, 8080);
        _tcpListener = new TcpListener(ipEndPoint);
        _sqlOperations = new SQLOperations();
    }

    public async Task ListenForClients()
    {
        _tcpListener.Start();
        Console.WriteLine("Listening for connections");
        while (true)
        {
            var clientId = await _tcpListener.AcceptTcpClientAsync();
            Console.WriteLine($"Client connected: {clientId.Client.RemoteEndPoint}");
            _ = HandleNewConnection(clientId);
        }
    }
    
    private async Task HandleNewConnection(TcpClient clientId)
    {
        var stream = clientId.GetStream();
        var buffer = new byte[1024];
        Client? client = null;
        _tcpClients.Add(clientId);
        try
        {
            while (true)
            {
                var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                if (bytesRead == 0)
                {
                    Console.Write($"Client disconnected: {clientId.Client.RemoteEndPoint}");
                    break;
                }
                var jsonString = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Message received: {jsonString}");
                JsonSerializerOptions options =new() { AllowOutOfOrderMetadataProperties = true };
                var message = JsonSerializer.Deserialize<Message>(jsonString,options);
                switch (message)
                {
                    case SignupMessage signupMsg:
                        await HandleSignup(signupMsg, stream);
                        break;
                    case ChatMessage chatMsg:
                        switch (chatMsg.ChatType)
                        {
                            case "JOIN":
                                //client = await HandleJoinClient(clientID, chatMsg);
                                break;
                            case "CHAT":
                                await HandleChatMessage(client, chatMsg);
                                break;
                        }

                        break;
                    case LoginRequestMessage loginMsg:
                    {
                        var result = await HandleLogin(loginMsg, stream);
                        if (result != true) continue;
                        client = new Client(clientId, loginMsg.Username);
                        lock(_clientsLock){
                            _loggedInClients.Add(client);
                        }
                        Console.WriteLine($"Client logged in: {client.Name}");
                        break;
                    }
                    case CreateGroupMessage createGroupMsg:
                        await HandleCreateGroup(createGroupMsg, stream);
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            DisconnectClient(clientId);
        }
    }
    
    private async Task HandleChatMessage(Client? client, ChatMessage message)
    {
        if(client != null){
            var messageObj = new ChatMessage {ChatType = "CHAT", Sender = message.Sender, chatMessage = message.chatMessage, UserList = GetUserList()};
            await BroadcastMessage(messageObj);
        }
    }

    private async Task<bool> HandleLogin(LoginRequestMessage requestMessage, NetworkStream stream)
    {
        const string sql = @"SELECT u.id AS user_id, u.password AS password, g.name AS group_name, g.Code AS group_code " +
                            "FROM ChatSchema.Users AS u " +
                            "LEFT JOIN ChatSchema.UserGroup AS ug " +
                            "ON u.id = ug.UserID " + 
                            "LEFT JOIN ChatSchema.Groups AS g " + 
                            "ON ug.GroupCode = g.Code " +
                            "WHERE u.username = @username";
        var responseMessage = "";
        var (connectMessage, password,success) = await _sqlOperations.SendSQLLogin(sql, requestMessage);
        if (!success)
        {
            Console.WriteLine("Login failed");
            responseMessage = "401 Unauthorized";
        }
        else
        {
            var isCorrect = Util.CheckPassword(requestMessage.Password,password);
            responseMessage = isCorrect ? "201 User Login" : "400 Bad Request";
        }
        connectMessage.Response = responseMessage;
        string jsonString = JsonSerializer.Serialize(connectMessage);
        Console.WriteLine($"Message received: {jsonString}");
        
                    
        await SendResponseMessage(jsonString, stream);
        return success;
    }

    private async Task<Client?> HandleConnectClient(TcpClient clientID, ConnectMessage message)
    {
        return new Client(clientID, message.Username);
    }
    
    private async Task HandleSignup(SignupMessage message, NetworkStream stream)
    {
        Console.WriteLine("Sign up request received");

        const string sql = "INSERT INTO ChatSchema.Users (Username, Password) VALUES  (@username, @password)";
        var responseMessage = "";
        if (!_sqlOperations.SendSQLSignup(sql, message))
        {
            Console.WriteLine("Sign up failed");
            responseMessage = "401 Unauthorized";
        }
        else
        {
            responseMessage = "201 User registered";
        }
                    
        await SendResponseMessage(responseMessage, stream);
    }
    
    private async Task<bool> HandleCreateGroup(CreateGroupMessage createGroupMsg, NetworkStream stream)
    {
        var groupName = createGroupMsg.groupName;
        var response = false;
        const string sql = "INSERT INTO ChatSchema.Groups (Name, Code)  VALUES (@Name, @Code)";
        for (var i = 0; i < 5; i++)
        {
            var groupCode = Util.GenrateRandomString();
            response = _sqlOperations.SendNewGroup(sql, groupName, groupCode);
            if (response)
            {
                break;
            }
        }

        var responseMessage = "";
        if (response)
        {
            responseMessage = "201 User registered";
            await SendResponseMessage(responseMessage, stream);
        }
        else
        {
            responseMessage = "400 Bad Request";
            await SendResponseMessage(responseMessage, stream);
        }
        return response;
    }


    //Broadcast everyone to currently connected Chat
    private Task BroadcastMessage(ChatMessage? message)
    {
        if (message.ChatType == "JOIN")
        {
            message.chatMessage = $"{message.Sender} has joined!";
            
        }
        else if (message.ChatType == "CHAT")
        {
            message.chatMessage = $"{message.Sender}: {message.chatMessage}";
        }
        var jsonString = JsonSerializer.Serialize(message);
        var messageBytes = Encoding.UTF8.GetBytes(jsonString);
        lock (_clientsLock)
        {
            foreach (var client in _loggedInClients)
            {
                var clientId = client?.ClientID;
                var name = client?.Name;
                var stream = clientId?.GetStream();
                Console.WriteLine($"Sending message: {message.chatMessage} to {clientId?.Client.RemoteEndPoint} | {name}");
                stream?.Write(messageBytes, 0, messageBytes.Length);
            }
        }

        return Task.CompletedTask;
    }

    private async Task HandleJoinGroup()
    {
        
    }

    private void DisconnectClient(TcpClient client)
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
        lock (_clientsLock)
        {
            return _loggedInClients.Select(clientName => clientName.Name).ToArray();
        }
    }

    private async Task SendResponseMessage(string response, NetworkStream stream)
    {
        var messageBytes = Encoding.UTF8.GetBytes(response);
        await stream.WriteAsync(messageBytes);
    }
}