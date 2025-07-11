using ChatroomDesktop.Models;
using ChatroomDesktop.Services;
using ChatroomDesktop.Services.Interfaces;
using ChatroomDesktop.Views;
using Message = ChatroomDesktop.Models.Message;

namespace ChatroomDesktop.Presenter;

public class SignupPresenter : BasePresenter<ISignupView>
{
     private readonly ISignupView _view;
     
     
     private INetworkService _networkService;
     private INavigatorService _navigatorService;
     private IChatService _chatService;
     private IMessageService _messageService;
     public SignupPresenter(ISignupView view, INetworkService networkService, INavigatorService navigatorService,
          IChatService chatService, IMessageService messageService) : base(view)
     {
          this._view = view;
          _view.CancelClicked += OnCancelClicked;
          _view.EnterClicked += OnEnterClicked;
          _networkService = networkService;
          _navigatorService = navigatorService;
          _chatService = chatService;
          _messageService = messageService;
     }

     private async void OnEnterClicked(object? sender, EventArgs e)
     {
          var result = await _networkService.SendSignupData(_view.Username, _view.Password);
          if (result != null)
          {
               Console.WriteLine(result);
               _view.HideForm();
               _navigatorService.OpenChatroomListPage(_chatService, _networkService, result);
          }
          else
          {
               _messageService.ShowMessage("Invalid username. Pick another one");
          }
     }

     private void OnCancelClicked(object? sender, EventArgs e)
     {
          _view.HideForm();
          _navigatorService.OpenLoginPage(_networkService, _chatService, _navigatorService, _messageService);
     }
}