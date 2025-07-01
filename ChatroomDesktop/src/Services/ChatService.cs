using ChatroomDesktop.Models;
using Message = ChatroomDesktop.Models.Message;

namespace ChatroomDesktop.Services;

public class ChatService
{

    private NetworkService _networkService;

    private SemaphoreSlim _sendLock;
    
    private Queue<string> _chatQueue = new Queue<string>();
    
    private CancellationTokenSource _cts;
    
    // The Point of this task is to await on it during cleanup so any leftover messages are sent before shutdown
    private Task _sendingTask;
    
    public event Action<ChatMessage> OnNewMessage;
     
    public event Action<ChatMessage> OnNewUser;
    public ChatService(NetworkService networkService)
    {
        this._networkService = networkService;
        _networkService.OnMessageReceived += ReceiveMessage;
        _sendLock = new SemaphoreSlim(0);
        _cts = new CancellationTokenSource();
    }

    public void HandleUserInput(string input)
    {
            _chatQueue.Enqueue(input);
            _sendLock.Release();
    }

    public async Task ReadyQueue()
    {
        _sendingTask = ProcessQueue(_cts.Token);
        Console.WriteLine("Queue has Closed");
    }

    private void ReceiveMessage(Message message)
    {
        if (message is ChatMessage chatMsg)
        {
            if (chatMsg.ChatType == "CHAT")
            {
                OnNewMessage?.Invoke(chatMsg);
            }
            else if (chatMsg.ChatType == "JOIN")
            {
                OnNewUser?.Invoke(chatMsg);
                OnNewMessage?.Invoke(chatMsg);
            }
            else if (chatMsg.ChatType == "DISCONNECT")
            {
                OnNewUser?.Invoke(chatMsg);
                OnNewMessage?.Invoke(chatMsg);
            } 
        }
    }

    private async Task ProcessQueue(CancellationToken token)
    {
        
        while (!token.IsCancellationRequested)
        {
            await _sendLock.WaitAsync(token);
            if (_chatQueue.TryDequeue(out var message))
            {
                await SendMessage(message);
            }
        }
        Console.WriteLine("Token Canceled");
    }

    private async Task SendMessage(string message)
    {
        Console.WriteLine($"Chat Service: Sending message: {message}");
        await _networkService.SendMessage(message);
    }

    public async Task CloseConnection()
    {
        Console.WriteLine("Closing connection");
        _networkService.CloseConnection();
        _cts.Cancel();
        //await _sendingTask;
        _cts.Dispose();
    }

}
