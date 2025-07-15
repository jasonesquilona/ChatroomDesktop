using ChatroomDesktop.Models;
using ChatroomDesktop.Presenter;
using ChatroomDesktop.Services.Interfaces;
using ChatroomDesktop.Views;

namespace ChatroomDesktop.Services;

public class NavigatorService : INavigatorService
{
    private IPresenter _currentPresenter;
    
    public void OpenSignupPage(INetworkService networkService, IChatService chatService,
        INavigatorService navigatorService, IMessageService messageService)
    {
        var signupForm = new SignupForm();
        _currentPresenter.Destroy();
        var signupPresenter = new SignupPresenter(signupForm, networkService, navigatorService, chatService,  messageService);
        _currentPresenter = signupPresenter;
        signupForm.Show();
    }

    public void OpenChatroomListPage(IChatService chatService, INetworkService networkService, UserModel user, INavigatorService navigatorService, IMessageService messageService)
    {
        var mainView = new GroupChatsForm(chatService);
        var groupListPresenter = new GroupChatListPresenter(mainView, networkService, chatService, user, navigatorService, messageService);
        _currentPresenter.Destroy();
        mainView.SetPresenter(groupListPresenter);
        _currentPresenter = groupListPresenter;
        mainView.Show();
    }

    public void OpenChatroomPage()
    {
    }

    public void OpenLoginPage(INetworkService networkService, IChatService chatService, INavigatorService navigatorService, IMessageService messageService)
    {
        var loginForm = new LoginForm();
        _currentPresenter.Destroy();
        var loginPresenter = new LoginPresenter(loginForm, networkService, messageService, navigatorService, chatService);
        _currentPresenter = loginPresenter;
        loginForm.Show();
    }

    public void SetPresenter(IPresenter presenter)
    {
        _currentPresenter = presenter;
    }
}