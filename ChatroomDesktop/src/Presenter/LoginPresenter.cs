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
        _networkService = networkService;
    }

    private async void OnEnterClicked(object sender, EventArgs e)
    {
        _user.Username = _view.Name;
        Console.WriteLine(_user.Username);
        await _networkService.SetUpConnection(_user.Username);
        _view.CloseForm();
    }
}