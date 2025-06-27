using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using ChatroomDesktop.Models;
using ChatroomDesktop.Utilities;
using Microsoft.VisualBasic.CompilerServices;
using Message = ChatroomDesktop.Models.Message;

namespace ChatroomDesktop.Services;

public class NetworkService
{
    private TcpClient _tcpClient;
    private IPEndPoint _ipEndPoint;

    private SemaphoreSlim _sendLock;

    public event Action<Message> OnMessageReceived;
    
    private CancellationTokenSource _cts;
    
    private UserModel _userModel;
    
    
    private bool _isConnected;

    public NetworkService()
    {
        this._tcpClient = new();
        this._sendLock = new(1);
        this._cts = new();
        this._userModel = new();
        this._isConnected = false;
    }

    public async Task SetUpConnection(string name)
    {
        if (!this._isConnected)
        {
            await ConnectToServer();
        }
        _userModel.Username = name;
    }

    public async Task<bool> SendSignupData(string username, string password)
    {
        if (!this._isConnected)
        {
            await ConnectToServer();
        }
        
        var stream = _tcpClient.GetStream();
        var hashedPassword = Util.SaltHashPassword(password);
        var message = new SignupMessage{Username = username, Password = hashedPassword};
        var data = ConvertToJson(message);
        
        await stream.WriteAsync(data, 0, data.Length);
        
        byte[] responseBuffer = new byte[1024]; // Adjust size as needed
        Console.WriteLine($"Sent Signup Request to Server!");
        int bytesRead = await stream.ReadAsync(responseBuffer, 0, responseBuffer.Length);
        
        string response = Encoding.UTF8.GetString(responseBuffer, 0, bytesRead);
        if (response.StartsWith("201"))
        {
            Console.WriteLine("Signed up!");
            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task HandleIncomingMessages()
    {
        
        var buffer = new byte[1024];

        try
        {
            var stream = _tcpClient.GetStream();
            while (_cts.Token.IsCancellationRequested == false)
            {
                var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, _cts.Token);
                if (bytesRead > 0)
                {
                    var jsonString = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Message message = JsonSerializer.Deserialize<Message>(jsonString);
                    OnMessageReceived?.Invoke(message);
                }
            }
        }
        catch (Exception ex)
        {
            if (ex is OperationCanceledException)
            {
                CloseStream();   
            }
            else
            {
                Console.WriteLine("Connection closed or error:" + ex.Message);
            }
        }

    }

    private void CloseStream()
    {
        var stream = _tcpClient.GetStream();
        stream.Close();
        this._tcpClient.Close();
        Console.WriteLine("Connection has been fully closed");
    }

    public async Task SendMessage(string message)
    {
       var messageObj = new ChatMessage{ChatType = "CHAT", Sender = _userModel.Username, chatMessage = message};
       var data = ConvertToJson(messageObj);
       var stream = _tcpClient.GetStream();
       await _sendLock.WaitAsync();
       Console.WriteLine($"Sending message: {messageObj}");
       try
       {
           await stream.WriteAsync(data, 0, data.Length);
           stream.Flush();
       }
       finally
       {
           _sendLock.Release();
       }
    }

    private static byte[] ConvertToJson(Message messageObj)
    {
        string jsonString = JsonSerializer.Serialize(messageObj);
        Console.WriteLine(jsonString);
        var data = Encoding.UTF8.GetBytes(jsonString);
        return data;
    }

    public async Task<bool> SendGroupCreationRequest(CreateGroupMessage message)
    {
        var data = ConvertToJson(message);
        var stream = _tcpClient.GetStream();
        await _sendLock.WaitAsync();
        await stream.WriteAsync(data, 0, data.Length);
        byte[] responseBuffer = new byte[1024]; // Adjust size as needed
        int bytesRead = await stream.ReadAsync(responseBuffer, 0, responseBuffer.Length);
        
        string response = Encoding.UTF8.GetString(responseBuffer, 0, bytesRead);
        if (response.StartsWith("201"))
        {
            Console.WriteLine("Group Created");
            return true;
        }
        else
        {
            return false;
        }
    }

    public void CloseConnection()
    {
        _cts.Cancel();
        Console.WriteLine("Connection closed");
    }

    public async Task ConnectToServer()
    {
        var hostName = Dns.GetHostName();
        Console.WriteLine("Connecting...");
        IPHostEntry localhost = await Dns.GetHostEntryAsync(Dns.GetHostName());
        // This is the IP address of the local machine
        IPAddress localIpAddress = localhost.AddressList[0];

        if (localIpAddress != null)
        {
            var ipEndPoint = new IPEndPoint(localIpAddress, 8080);
            Console.WriteLine("Attempting to connect to server...");
            await _tcpClient.ConnectAsync(ipEndPoint).ConfigureAwait(false);
        }

        _isConnected = true;
    }

    public async Task<ConnectMessage> CheckCredentials(String username, String password)
    {
        var stream = _tcpClient.GetStream();
        var message = new LoginRequestMessage{Username = username, Password= password};
        Console.WriteLine("Logging in..");
        var data = ConvertToJson(message);
        
        await stream.WriteAsync(data, 0, data.Length);
        
        byte[] responseBuffer = new byte[1024]; // Adjust size as needed
        Console.WriteLine($"Sent Login Request to Server!");
        int bytesRead = await stream.ReadAsync(responseBuffer, 0, responseBuffer.Length);
        string response = Encoding.UTF8.GetString(responseBuffer, 0, bytesRead);
        JsonSerializerOptions options =new() { AllowOutOfOrderMetadataProperties = true };
        var connectMessage = JsonSerializer.Deserialize<ConnectMessage>(response,options);
        
        if (connectMessage.Response.StartsWith("201"))
        {
            Console.WriteLine("Logged in!");
            return connectMessage;
        }
        else
        {
            Console.WriteLine("Invalid username or password!");
            return null;
        }
    }
}