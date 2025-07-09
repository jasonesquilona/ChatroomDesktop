using ChatroomDesktop.Models;

namespace ChatroomDesktop.Services.Interfaces;

public interface INavigatorService
{
    void OpenSignupPage(INetworkService networkService, IChatService chatService, INavigatorService navigatorService);
    
    void OpenChatroomPage();

    void OpenChatroomListPage(IChatService chatService, INetworkService networkService, UserModel user);
    
    void OpenLoginPage();
}