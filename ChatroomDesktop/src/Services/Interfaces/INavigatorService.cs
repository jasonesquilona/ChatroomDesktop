using ChatroomDesktop.Models;

namespace ChatroomDesktop.Services.Interfaces;

public interface INavigatorService
{
    void OpenSignupPage(INetworkService networkService, ChatService chatService, INavigatorService navigatorService);
    
    void OpenChatroomPage();

    void OpenChatroomListPage(ChatService chatService, INetworkService networkService, UserModel user);
    
    void OpenLoginPage();
}