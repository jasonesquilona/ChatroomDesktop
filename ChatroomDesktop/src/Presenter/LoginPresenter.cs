using ChatroomDesktop.Models;
using ChatroomDesktop.Services;
using ChatroomDesktop.Views;
using Microsoft.VisualBasic.ApplicationServices;

namespace ChatroomDesktop.Presenter;

public class LoginPresenter
{
    private readonly INameView _view;
    private readonly UserModel _user;
    private NetworkService _networkService;
    

    public LoginPresenter(INameView view, UserModel user, NetworkService networkService)
    {
        _view = view;
        _user = user;
        _view.EnterClicked += OnEnterClicked;
        _view.SignUpClicked += OnSignUpClicked;
        _networkService = networkService;
    }
    

    private async void OnEnterClicked(object sender, EventArgs e)
    {
        await SuccessfulLogin(_view.Name);
    }

    private async Task SuccessfulLogin(string name)
    {
        _user.Username = name;
        Console.WriteLine(_user.Username);
        await _networkService.SetUpConnection(_user.Username);
        
          
        var mainView = new ChatroomForm();
        var chatModel = new ChatModel();
        ChatService chatService = new ChatService(_networkService);
        var chatroomPresenter = new ChatroomPresenter(mainView,chatModel,_networkService,chatService, _user);
        var recieve = chatService.ReadyQueue();
        var listen=  _networkService.HandleIncomingMessages();
        Console.WriteLine("Opening Chatroom...");
        mainView.Show();
        ((Form)_view).Hide();

        await Task.WhenAll(listen, recieve);
        _view.CloseForm();
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

    private void CheckCredentials(string username, string password)
    {
        
    }
    
}