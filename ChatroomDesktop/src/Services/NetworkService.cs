using System.Net;
using System.Net.Sockets;
using System.Text;
using ChatroomDesktop.Models;

namespace ChatroomDesktop.Services;

public class NetworkService
{
    private TcpClient _tcpClient;
    private IPEndPoint _ipEndPoint;

    private SemaphoreSlim _sendLock;

    public event Action<String> OnMessageReceived;
    
    private CancellationTokenSource _cts;
    
    private User _user;
    
    public NetworkService()
    {
        this._tcpClient = new();
        this._sendLock = new(1);
        this._cts = new();
        this._user = new();
    }

    public async Task SetUpConnection()
    {
        var hostName = Dns.GetHostName();
        IPHostEntry localhost = await Dns.GetHostEntryAsync(hostName);
        // This is the IP address of the local machine
        IPAddress localIpAddress = localhost.AddressList[0];
        
        var ipEndPoint = new IPEndPoint(localIpAddress, 8080);
        Console.WriteLine("Hello, Welcome to the Chatroom! What is your username?");
        var name = Console.ReadLine();
        Console.WriteLine("Connecting...");
        await _tcpClient.ConnectAsync(ipEndPoint, _cts.Token);
        
        var stream = _tcpClient.GetStream();
        var data = Encoding.UTF8.GetBytes(name);
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
                    var message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    OnMessageReceived?.Invoke(message);
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