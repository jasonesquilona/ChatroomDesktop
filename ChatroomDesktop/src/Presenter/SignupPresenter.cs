using ChatroomDesktop.Models;
using ChatroomDesktop.Services;
using ChatroomDesktop.Views;

namespace ChatroomDesktop.Presenter;

public class SignupPresenter
{
     private readonly ISignupView _view;

     public event EventHandler? FormClosed;
     
     private NetworkService _networkService;
     public SignupPresenter(ISignupView view, NetworkService networkService)
     {
          this._view = view;
          _view.CancelClicked += OnCancelClicked;
          _view.EnterClicked += OnEnterClicked;
          _networkService = networkService;
     }

     private async void OnEnterClicked(object? sender, EventArgs e)
     {
          await _networkService.SendSignupData(_view.Username, _view.Password);
          ((Form)_view).Hide();
          FormClosed?.Invoke(this, new SignUpEventArgs(isSignupSuccess: true, _view.Username));
     }

     private void OnCancelClicked(object? sender, EventArgs e)
     {
          ((Form)_view).Hide();
          FormClosed?.Invoke(this, EventArgs.Empty);
     }
}