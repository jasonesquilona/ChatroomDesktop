using System.Net.Sockets;
using ChatroomDesktop.Services;

namespace ChatroomDesktop;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    ///[STAThread]
    static async Task Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        //ApplicationConfiguration.Initialize();
        //Application.Run(new Form1());
        
        
        NetworkService networkService = new NetworkService();
        ChatService chatService = new ChatService(networkService);

        await networkService.SetUpConnection();

        Console.WriteLine("Hello Welcome to Chatroom Desktop! Type to send message");
        var send = chatService.HandleUserInput();
        var listen=  networkService.HandleIncomingMessages();
        
        
        await Task.WhenAll(listen, send);
        
        Console.WriteLine("Chatroom Closing");

    }
}