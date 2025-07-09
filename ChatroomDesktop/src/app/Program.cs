using System.Net.Sockets;
using ChatroomDesktop.Models;
using ChatroomDesktop.Presenter;
using ChatroomDesktop.Services;
using ChatroomDesktop.Views;
using Message = ChatroomDesktop.Models.Message;

namespace ChatroomDesktop;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        RunAsync().GetAwaiter().GetResult();
    }

    static async Task RunAsync()
    {
        var view = new LoginForm();
        var user = new UserModel();
        var navigatorService = new NavigatorService();
        NetworkService networkService = new NetworkService();
        MessageService messageService = new MessageService();
        ChatService chatService = new ChatService(networkService);
        //await networkService.ConnectToServer();
        var presenter = new LoginPresenter(view, user, networkService, messageService, navigatorService, chatService);
        view.SetPresenter(presenter);
        Application.Run((Form)view);
    }
}