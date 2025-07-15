using ChatroomDesktop.Models;
using ChatroomDesktop.Services;
using ChatroomDesktop.Services.Interfaces;
using ChatroomDesktop.Views;
using Microsoft.VisualBasic.ApplicationServices;

namespace ChatroomDesktop.Presenter;

public class LoginPresenter : BasePresenter<ILoginView>
{
    private ILoginView _view;
    private UserModel _user;
    private INetworkService _networkService;
    private IMessageService _messageService;
    private INavigatorService _navigatorService;
    private IChatService _chatService;
    

    public LoginPresenter(ILoginView view, INetworkService networkService,
        IMessageService messageService, INavigatorService navigatorService, IChatService chatService) : base(view)
    {
        _view = view;
        _view.EnterClicked += OnEnterClicked;
        _view.SignUpClicked += OnSignUpClicked;
        _messageService = messageService;
        _networkService = networkService;
        _chatService = chatService;
        _navigatorService = navigatorService;
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
            _messageService.ShowMessage("Wrong username or password!");
        }
    }

    private async Task SuccessfulLogin(UserModel user)
    {
        try
        {
            this._user = user;
            Console.WriteLine(_user.Details.UserId);
            await _networkService.SetUpConnection();
            
            //var mainView = new ChatroomForm();
            //var chatroomPresenter = new ChatroomPresenter(mainView,chatModel,_networkService,chatService, _user);
            //var recieve = chatService.ReadyQueue();
            //var listen=  _networkService.HandleIncomingMessages();
            Console.WriteLine("Opening Group Chats Form...");
            _navigatorService.OpenChatroomListPage(_chatService,_networkService, _user, _navigatorService, _messageService);
            _view.HideForm();
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
        _navigatorService.OpenSignupPage(_networkService, _chatService, _navigatorService, _messageService);
        _view.HideForm();
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