using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using ChatroomDesktop.Models;
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
        
        var stream = _tcpClient.GetStream();
        var message = new Message{MessageType="JOIN", Sender = name};
        string jsonString = JsonSerializer.Serialize(message);
        var data = Encoding.UTF8.GetBytes(jsonString);
        await stream.WriteAsync(data, 0, data.Length);
        Console.WriteLine($"Connected to Server!");
    }

    public async Task SendSignupData(string username, string password)
    {
        if (!this._isConnected)
        {
            await ConnectToServer();
        }
        /*
        var sslStream = new SslStream(_tcpClient.GetStream(), false,
            (sender, cert, chain, errors) => true);
        sslStream.AuthenticateAsClient("localhost");*/
        
        var stream = _tcpClient.GetStream();
        var message = new Message{MessageType="SIGNUP", Sender = username, ChatMessage = password};
        string jsonString = JsonSerializer.Serialize(message);
        var data = Encoding.UTF8.GetBytes(jsonString);
        
        await stream.WriteAsync(data, 0, data.Length);
        
        byte[] responseBuffer = new byte[1024]; // Adjust size as needed
        int bytesRead = await stream.ReadAsync(responseBuffer, 0, responseBuffer.Length);
        
        string response = Encoding.UTF8.GetString(responseBuffer, 0, bytesRead);
        if (response.StartsWith("201"))
        {
            Console.WriteLine("Signed up!");
        }
        else
        {
            
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
                    Console.WriteLine($"Received {message.ChatMessage}");
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
       var data = Encoding.UTF8.GetBytes(message);
       var stream = _tcpClient.GetStream();
       await _sendLock.WaitAsync();
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
        
        var ipEndPoint = new IPEndPoint(localIpAddress, 8080);
        await _tcpClient.ConnectAsync(ipEndPoint, _cts.Token);
        
        _isConnected = true;
    }
}