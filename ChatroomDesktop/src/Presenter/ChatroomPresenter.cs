using ChatroomDesktop.Models;
using ChatroomDesktop.Services;
using ChatroomDesktop.Views;
using Microsoft.VisualBasic.ApplicationServices;
using Message = ChatroomDesktop.Models.Message;

namespace ChatroomDesktop.Presenter;

public class ChatroomPresenter
{
    private readonly IChatroomView _view;
    private readonly ChatModel _chatModel;
    private readonly UserModel _user;
    private readonly NetworkService _networkService;
    private readonly ChatService _chatService;

    public ChatroomPresenter(IChatroomView view, ChatModel chatModel, NetworkService networkService, ChatService chatService, UserModel user)
    {
        _view = view;
        _chatModel = chatModel;
        _user = user;
        _networkService = networkService;
        _view.EnterClicked += OnEnterClicked;
        _view.FormClosed += OnFormClosed;
        _chatService = chatService;
        _chatService.OnNewMessage += HandleNewMessage;
        _chatService.OnNewUser += HandleNewUser;
    }

    private void OnFormClosed(object? sender, EventArgs e)
    {
        _chatService.CloseConnection();
    }

    private async void HandleNewMessage(ChatMessage obj)
    {
        _view.AddMessage(obj.chatMessage);
    }

    private async void HandleNewUser(ChatMessage obj)
    {
        _view.AddNewUser(obj.UserList);
    }

    private async void OnEnterClicked(object? sender, EventArgs e)
    {
        var message = _view.CurrentMessage;
        _view.ClearMessageBox();
        _chatService.HandleUserInput(message);
    }
}