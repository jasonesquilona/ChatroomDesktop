using ChatroomDesktop.Models;
using ChatroomDesktop.Presenter;

namespace ChatroomDesktop.Services.Interfaces;

public interface INavigatorService
{
    void OpenSignupPage(INetworkService networkService, IChatService chatService, INavigatorService navigatorService,
        IMessageService messageService);
    
    Task OpenChatroomPage(IChatService chatService, INetworkService networkService, UserModel user,
        INavigatorService navigatorService, IMessageService messageService, string groupId);

    void OpenChatroomListPage(IChatService chatService, INetworkService networkService, UserModel user,
        INavigatorService navigatorService, IMessageService messageService);

    void OpenLoginPage(INetworkService networkService, IChatService chatService, INavigatorService navigatorService,
        IMessageService messageService);

    void SetPresenter(IPresenter presenter);
}