using System.Net;
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
    
    public NetworkService()
    {
        this._tcpClient = new();
        this._sendLock = new(1);
        this._cts = new();
        this._userModel = new();
    }

    public async Task SetUpConnection(string name)
    {
        var hostName = Dns.GetHostName();
        IPHostEntry localhost = await Dns.GetHostEntryAsync(hostName);
        // This is the IP address of the local machine
        IPAddress localIpAddress = localhost.AddressList[0];
        
        var ipEndPoint = new IPEndPoint(localIpAddress, 8080);
        Console.WriteLine("Connecting...");
        await _tcpClient.ConnectAsync(ipEndPoint, _cts.Token);
        
        var stream = _tcpClient.GetStream();
        var message = new Message{MessageType="JOIN", Sender = name};
        string jsonString = JsonSerializer.Serialize(message);
        var data = Encoding.UTF8.GetBytes(jsonString);
        await stream.WriteAsync(data, 0, data.Length);
        Console.WriteLine($"Connected to Server!");
    }

    public async Task HandleIncomingMessages()
    {
        
        var buffer = new byte[1024];

        try
        {
            var stream = _tcpClient.GetStream();
            while (_cts.Token.IsCancellationRequested == false)
            {
                var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                if (bytesRead > 0) {
                    var jsonString = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Message message = JsonSerializer.Deserialize<Message>(jsonString);
                    OnMessageReceived?.Invoke(message);
                    Console.WriteLine($"Received {message.ChatMessage}");
                }           
            }
            stream.Close();
            this._tcpClient.Close();
        }   
        catch (Exception ex)
        {
            Console.WriteLine("Connection closed or error:" + ex.Message);
        }
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
        Console.WriteLine("Closing Connection");
        _cts.Cancel();
    }
}