using ChatroomDesktop.Models;
using ChatroomDesktop.Services;
using ChatroomDesktop.Views;
using Microsoft.VisualBasic.ApplicationServices;

namespace ChatroomDesktop.Presenter;

public class LoginPresenter
{
    private ILoginView _view;
    private UserModel _user;
    private NetworkService _networkService;
    

    public LoginPresenter(ILoginView view, UserModel user, NetworkService networkService)
    {
        _view = view;
        _user = user;
        _view.EnterClicked += OnEnterClicked;
        _view.SignUpClicked += OnSignUpClicked;
        _networkService = networkService;
    }
    

    private void OnEnterClicked(object sender, EventArgs e)
    {
        _ = HandleEnterClicked();
    }

    private async Task HandleEnterClicked()
    {
        UserModel result = await CheckCredentials(_view.Name, _view.Password);
        if (result != null)
        {
            
            await SuccessfulLogin(result);    
        }
        else
        {
            _view.IncorrectLoginDetails();
            MessageBox.Show("Wrong username or password!");
        }
    }

    private async Task SuccessfulLogin(UserModel user)
    {
        try
        {
            this._user = user;
            Console.WriteLine(_user.Username);
            await _networkService.SetUpConnection(_user.Username);
        
          
            //var mainView = new ChatroomForm();
            ChatService chatService = new ChatService(_networkService);
            var mainView = new GroupChatsForm(chatService);
            var groupListPrsenter = new GroupChatListPresenter(mainView, _networkService, chatService, user);
            //var chatroomPresenter = new ChatroomPresenter(mainView,chatModel,_networkService,chatService, _user);
            //var recieve = chatService.ReadyQueue();
            //var listen=  _networkService.HandleIncomingMessages();
            Console.WriteLine("Opening Group Chats Form...");
            mainView.SetPresenter(groupListPrsenter);
            _view.HideForm();
            mainView.Show();
            ///await Task.WhenAll(listen, recieve);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in SuccessfulLogin: {ex}");
            MessageBox.Show($"Login failed: {ex.Message}");
        }
    }

    private void OnSignUpClicked(object sender, EventArgs e)
    {
        var signupForm = new SignupForm();
        var signupPresenter = new SignupPresenter(signupForm, _networkService);
        signupPresenter.FormClosed += (sender, e) => OnSignUpClosed(sender, e as SignUpEventArgs);
        signupForm.Show();
        _view.HideForm();
    }

    private async Task OnSignUpClosed(object? sender,SignUpEventArgs e)
    {
        if(e.isSignupSuccess)
        {
            Console.WriteLine("Signup successful!");
            UserModel userModel = new UserModel();
            userModel.Username = e.msg.Username;
            userModel.UserId = e.msg.Userid;
            userModel.Groups = e.msg.GroupList;
            try
            {
                await SuccessfulLogin(userModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Handler error: {ex}");
            }
        }
        else
        {
            _view.ShowForm();
        }
    }

    private async Task<UserModel> CheckCredentials(string username, string password)
    {
        return await _networkService.CheckCredentials(username, password);
    }

    public async Task SetupConnection()
    {
        await _networkService.ConnectToServer();
    }
}