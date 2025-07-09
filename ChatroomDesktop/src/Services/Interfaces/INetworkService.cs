using ChatroomDesktop.Models;
using Message = ChatroomDesktop.Models.Message;

namespace ChatroomDesktop.Services.Interfaces;

public interface INetworkService
{
    Task<UserModel?> CheckCredentials(string username, string password);
    Task SetUpConnection(string username);
    Task ConnectToServer();
    
    event Action<Message> OnMessageReceived;

    Task<ConnectMessage> SendSignupData(string username, string password);

    Task SendMessage(string message);

    Task<bool> SendGroupCreationRequest(CreateGroupMessage message);

    void CloseConnection();

    Task<string> SendJoinGroupRequest(UserModel _userModel, string groupCode);
}