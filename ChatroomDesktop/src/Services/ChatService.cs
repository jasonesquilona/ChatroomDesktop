namespace ChatroomDesktop.Services;

public class ChatService
{

    private NetworkService _networkService;

    private SemaphoreSlim _sendLock;
    
    private Queue<string> _chatQueue = new Queue<string>();

    public ChatService(NetworkService networkService)
    {
        this._networkService = networkService;
        _networkService.OnMessageReceived += ReceiveMessage;
        _sendLock = new SemaphoreSlim(0);
    }

    public async Task HandleUserInput()
    {

        _ = Task.Run(ProcessQueue);
        while (true)
        {

            string input = await Task.Run(() => Console.ReadLine());
            _chatQueue.Enqueue(input);
            _sendLock.Release();
        }
    }

    private void ReceiveMessage(string message)
    {
        Console.WriteLine(message);
    }

    private async Task ProcessQueue()
    {
        while (true)
        {
            await _sendLock.WaitAsync();
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
    }

}