using ChatroomDesktop.Models;
using ChatroomDesktop.Services;
using ChatroomDesktop.Services.Interfaces;
using ChatroomDesktop.Views;
using Message = ChatroomDesktop.Models.Message;

namespace ChatroomDesktop.Presenter;

public class SignupPresenter
{
     private readonly ISignupView _view;

     public event EventHandler? FormClosed;
     
     private INetworkService _networkService;
     public SignupPresenter(ISignupView view, INetworkService networkService)
     {
          this._view = view;
          _view.CancelClicked += OnCancelClicked;
          _view.EnterClicked += OnEnterClicked;
          _networkService = networkService;
     }

     private async void OnEnterClicked(object? sender, EventArgs e)
     {
          var result = await _networkService.SendSignupData(_view.Username, _view.Password);
          if (result != null)
          {
               Console.WriteLine(result);
               _view.HideForm();
               FormClosed?.Invoke(this, new SignUpEventArgs(isSignupSuccess: true, result));
          }
          else
          {
               MessageBox.Show("Invalid username. Pick another one");
          }
     }

     private void OnCancelClicked(object? sender, EventArgs e)
     {
          _view.HideForm();
          FormClosed?.Invoke(this, EventArgs.Empty);
     }
}