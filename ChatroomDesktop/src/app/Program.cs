using System.Net.Sockets;
using ChatroomDesktop.Models;
using ChatroomDesktop.Presenter;
using ChatroomDesktop.Services;
using ChatroomDesktop.Views;

namespace ChatroomDesktop;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static async Task Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();

        var view = new NameForm();
        var user = new UserModel();
        NetworkService networkService = new NetworkService();
        Console.WriteLine("Connecting...");
        //await networkService.ConnectToServer();
        var presenter = new LoginPresenter(view, user, networkService);
        Application.Run((Form)view);
        
        /*
        NetworkService networkService = new NetworkService();
        ChatService chatService = new ChatService(networkService);

        await networkService.SetUpConnection();

        Console.WriteLine("Hello Welcome to Chatroom Desktop! Type to send message");
        var send = chatService.HandleUserInput();
        var listen=  networkService.HandleIncomingMessages();


        await Task.WhenAll(listen, send);

        Console.WriteLine("Chatroom Closing");*/

    }
}