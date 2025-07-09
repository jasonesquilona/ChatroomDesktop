using ChatroomDesktop.Models;
using ChatroomDesktop.Services;
using ChatroomDesktop.Services.Interfaces;
using ChatroomDesktop.Views;
using Message = ChatroomDesktop.Models.Message;

namespace ChatroomDesktop.Presenter;

public class SignupPresenter
{
     private readonly ISignupView _view;
     
     
     private INetworkService _networkService;
     private INavigatorService _navigatorService;
     private ChatService _chatService;
     public SignupPresenter(ISignupView view, INetworkService networkService, INavigatorService navigatorService, ChatService chatService)
     {
          this._view = view;
          _view.CancelClicked += OnCancelClicked;
          _view.EnterClicked += OnEnterClicked;
          _networkService = networkService;
          _navigatorService = navigatorService;
          _chatService = chatService;
     }

     private async void OnEnterClicked(object? sender, EventArgs e)
     {
          var result = await _networkService.SendSignupData(_view.Username, _view.Password);
          if (result != null)
          {
               Console.WriteLine(result);
               _view.HideForm();
               UserModel userModel = new UserModel();
               userModel.Username = result.Username;
               userModel.UserId = result.Userid;
               userModel.Groups = result.GroupList;
               _navigatorService.OpenChatroomListPage(_chatService, _networkService, userModel);
          }
          else
          {
               MessageBox.Show("Invalid username. Pick another one");
          }
     }

     private void OnCancelClicked(object? sender, EventArgs e)
     {
          _view.HideForm();
          _navigatorService.OpenLoginPage();
     }
}