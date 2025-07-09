using ChatroomDesktop.Models;
using ChatroomDesktop.Presenter;
using ChatroomDesktop.Services.Interfaces;
using ChatroomDesktop.Views;

namespace ChatroomDesktop.Services;

public class NavigatorService : INavigatorService
{
    public void OpenSignupPage(INetworkService networkService, IChatService chatService, INavigatorService navigatorService)
    {
        var signupForm = new SignupForm();
        var signupPresenter = new SignupPresenter(signupForm, networkService, navigatorService, chatService);
        signupForm.Show();
    }

    public void OpenChatroomListPage(IChatService chatService, INetworkService networkService, UserModel user)
    {
        var mainView = new GroupChatsForm(chatService);
        var groupListPrsenter = new GroupChatListPresenter(mainView, networkService, chatService, user);
        mainView.SetPresenter(groupListPrsenter);
        mainView.Show();
    }

    public void OpenChatroomPage()
    {
        throw new NotImplementedException();
    }

    public void OpenLoginPage()
    {
        throw new NotImplementedException();
    }
}