using Message = ChatroomDesktop.Models.Message;

namespace ChatroomDesktop.Services;

public class ChatService
{

    private NetworkService _networkService;

    private SemaphoreSlim _sendLock;
    
    private Queue<string> _chatQueue = new Queue<string>();
    
    private CancellationTokenSource _cts;
    
    public event Action<Message> OnNewMessage;
    
    public event Action<Message> OnNewUser;
    public ChatService(NetworkService networkService)
    {
        this._networkService = networkService;
        _networkService.OnMessageReceived += ReceiveMessage;
        _sendLock = new SemaphoreSlim(0);
        _cts = new CancellationTokenSource();
        _ = Task.Run(() => ProcessQueue(_cts.Token));
    }

    public async Task HandleUserInput(string input)
    {
            //string input = await Task.Run(() => Console.ReadLine());
            _chatQueue.Enqueue(input);
            _sendLock.Release();
    }

    private void ReceiveMessage(Message message)
    {
        if (message.MessageType == "CHAT")
        {
            OnNewMessage?.Invoke(message);
        }
        else if (message.MessageType == "JOIN")
        {
            OnNewUser?.Invoke(message);
            OnNewMessage?.Invoke(message);
        }
        //Console.WriteLine(message);
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
    }

    private async Task SendMessage(string message)
    {
        await _networkService.SendMessage(message);
    }

    public void CloseConnection()
    {
        _networkService.CloseConnection();
        _cts.Cancel();
    }

}
