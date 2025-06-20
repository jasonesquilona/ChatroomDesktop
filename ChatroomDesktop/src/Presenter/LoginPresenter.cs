using ChatroomDesktop.Models;
using ChatroomDesktop.Services;
using ChatroomDesktop.Views;
using Microsoft.VisualBasic.ApplicationServices;

namespace ChatroomDesktop.Presenter;

public class LoginPresenter
{
    private readonly ILoginView _view;
    private readonly UserModel _user;
    private NetworkService _networkService;
    

    public LoginPresenter(ILoginView view, UserModel user, NetworkService networkService)
    {
        _view = view;
        _user = user;
        _view.EnterClicked += OnEnterClicked;
        _view.SignUpClicked += OnSignUpClicked;
        _networkService = networkService;
    }
    

    private async void OnEnterClicked(object sender, EventArgs e)
    {
        bool result = await CheckCredentials(_view.Name, _view.Password);
        if (result)
        {
            await SuccessfulLogin(_view.Name);    
        }
        else
        {
            _view.IncorrectLoginDetails();
            MessageBox.Show("Wrong username or password!");
        }
    }

    private async Task SuccessfulLogin(string name)
    {
        _user.Username = name;
        Console.WriteLine(_user.Username);
        await _networkService.SetUpConnection(_user.Username);
        
          
        //var mainView = new ChatroomForm();
        var mainView = new GroupChatsForm();
        var chatModel = new ChatModel();
        ChatService chatService = new ChatService(_networkService);
        var groupListPrsenter = new GroupChatListPresenter(mainView, _networkService, chatService);
        //var chatroomPresenter = new ChatroomPresenter(mainView,chatModel,_networkService,chatService, _user);
        //var recieve = chatService.ReadyQueue();
        //var listen=  _networkService.HandleIncomingMessages();
        Console.WriteLine("Opening Group Chats Form...");
        mainView.Show();
        ((Form)_view).Hide();

        ///await Task.WhenAll(listen, recieve);
    }

    private void OnSignUpClicked(object sender, EventArgs e)
    {
        var signupForm = new SignupForm();
        var signupPresenter = new SignupPresenter(signupForm, _networkService);
        signupForm.Show();
        signupPresenter.FormClosed += (sender, e) => OnSignUpClosed(sender, e as SignUpEventArgs);
        ((Form)_view).Hide();
    }

    private async void OnSignUpClosed(object? sender,SignUpEventArgs e)
    {
        if(e.isSignupSuccess)
        {
            await SuccessfulLogin(e.name);
        }
        else
        {
            ((Form)_view).Show();   
        }
    }

    private async Task<bool> CheckCredentials(string username, string password)
    {
        return await _networkService.CheckCredentials(username, password);
    }

    public async Task SetupConnection()
    {
        await _networkService.ConnectToServer();
    }
}