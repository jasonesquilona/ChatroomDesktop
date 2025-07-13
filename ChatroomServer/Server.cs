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
    private List<ClientModel?> _loggedInClients = new List<ClientModel?>();
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
        ClientModel? client = null;
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
                        client = new ClientModel(clientId, loginMsg.Username);
                        lock(_clientsLock){
                            _loggedInClients.Add(client);
                        }
                        Console.WriteLine($"Client logged in: {client.Name}");
                        break;
                    }
                    case CreateGroupMessage createGroupMsg:
                        await HandleCreateGroup(createGroupMsg, stream);
                        break;
                    case JoinGroupMessage joinGroupMsg:
                    {
                        var result = await HandleJoinGroup(joinGroupMsg, stream);
                        break;
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
            DisconnectClient(clientId);
        }
    }

    private async Task<bool> HandleJoinGroup(JoinGroupMessage joinGroupMsg, NetworkStream stream)
    {

        const string sql = "INSERT INTO ChatSchema.UserGroup (UserId, GroupCode,GroupName) " +
                           "OUTPUT inserted.GroupName "+
                           "SELECT @UserId, @GroupCode, Name " +
                           "FROM ChatSchema.Groups as g " +
                           "WHERE g.Code = @GroupCode";
        var responseMessage = "";
        var (confirmMessage, success) = await _sqlOperations.SendJoinGroup(sql, joinGroupMsg.UserId, joinGroupMsg.GroupCode);
        if (!success)
        {
            responseMessage = "401 Unauthorized";
        }
        else
        {
            responseMessage = "201 User registered"; 
        }
        confirmMessage.Response = responseMessage;
        string jsonString = JsonSerializer.Serialize(confirmMessage);
        Console.WriteLine($"Message received: {jsonString}");
        
        await SendResponseMessage(jsonString, stream);
        return success;
    }

    private async Task HandleChatMessage(ClientModel? client, ChatMessage message)
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

    private async Task<ClientModel?> HandleConnectClient(TcpClient clientID, JoinGroupMessage message)
    {
        return null;
    }
    
    private async Task HandleSignup(SignupMessage message, NetworkStream stream)
    {
        Console.WriteLine("Sign up request received");

        const string sql = "INSERT INTO ChatSchema.Users (Username, Password) " +
                           "OUTPUT inserted.id " +
                           "VALUES  (@username, @password) ";
        var responseMessage = "";
        var (connectMessage, success) = await _sqlOperations.SendSQLSignup(sql, message);
        if (!success)
        {
            Console.WriteLine("Sign up failed");
            responseMessage = "401 Unauthorized";
        }
        else
        {
            responseMessage = "201 User registered";
        }
        connectMessage.Response = responseMessage;
        string jsonString = JsonSerializer.Serialize(connectMessage);
        Console.WriteLine($"Message received: {jsonString}");
        
        await SendResponseMessage(jsonString, stream);
    }

    private async Task HandleCreateGroup(CreateGroupMessage createGroupMsg, NetworkStream stream)
    {
        var groupName = createGroupMsg.groupName;
        var response = false;
        const string sql = "INSERT INTO ChatSchema.Groups (Name, Code) "+
                           "VALUES (@Name, @Code); "+
                           "INSERT INTO ChatSchema.UserGroup (GroupName, GroupCode, UserID)" +
                           "VALUES (@Name, @Code, @UserID); ";
        string groupCode = "";
        for (var i = 0; i < 1; i++)
        {
            groupCode = Util.GenrateRandomString();
            response = _sqlOperations.SendCreateNewGroup(sql, groupName, groupCode, createGroupMsg.UserId);
            if (response)
            {
                break;
            }
        }
        var responseMessage = new ConfirmGroupJoinMessage {GroupName = groupName, GroupCode = groupCode};
        if (response)
        {
            responseMessage.Response = "201 User registered";
        }
        else
        {
            responseMessage.Response = "400 Bad Request";
        }
        string jsonString = JsonSerializer.Serialize(responseMessage);
        await SendResponseMessage(jsonString, stream);
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