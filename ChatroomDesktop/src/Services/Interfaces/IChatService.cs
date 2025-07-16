using ChatroomDesktop.Models;

namespace ChatroomDesktop.Services.Interfaces;

public interface IChatService
{
    Task CloseConnection();
    
    public event Action<ChatMessage> OnNewMessage;
     
    public event Action<ChatMessage> OnNewUser;

    public void HandleUserInput(ChatMessage message);
    public Task ReadyQueue();
}