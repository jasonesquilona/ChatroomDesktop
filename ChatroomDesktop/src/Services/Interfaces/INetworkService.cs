using ChatroomDesktop.Models;
using Message = ChatroomDesktop.Models.Message;

namespace ChatroomDesktop.Services.Interfaces;

public interface INetworkService
{
    Task<UserModel?> CheckCredentials(string username, string password);
    Task SetUpConnection();
    Task ConnectToServer();
    
    event Action<Message> OnMessageReceived;

    Task<UserModel> SendSignupData(string username, string password);

    Task SendMessage(string message);

    Task<bool> SendGroupCreationRequest(CreateGroupMessage message);

    void CloseConnection();

    Task<ConfirmGroupJoinMessage?> SendJoinGroupRequest(UserModel userModel, string groupCode);
    Task ConnectToGroupChat(string groupName, string groupId, UserModel user);
}