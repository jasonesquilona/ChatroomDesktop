using ChatroomDesktop.Models;
using ChatroomDesktop.Services;
using ChatroomDesktop.Services.Interfaces;
using ChatroomDesktop.Views;
using Microsoft.VisualBasic.ApplicationServices;
using Message = ChatroomDesktop.Models.Message;

namespace ChatroomDesktop.Presenter;

public class ChatroomPresenter : BasePresenter<IChatroomView>
{
    private readonly IChatroomView _view;
    private readonly ChatModel _chatModel;
    private readonly UserModel _user;
    private readonly INetworkService _networkService;
    private readonly IChatService _chatService;
    private readonly IMessageService _messageService;
    private readonly INavigatorService _navigatorService;
    

    public ChatroomPresenter(IChatroomView view, INetworkService networkService, INavigatorService navigatorService,
        IChatService chatService, UserModel user, IMessageService messageService, string groupId) : base(view)
    {
        _view = view;
        _chatModel = new ChatModel();
        _chatModel.GroupId = groupId;
        _user = user;
        _networkService = networkService;
        _view.EnterClicked += OnEnterClicked;
        _view.FormClosed += OnFormClosed;
        _chatService = chatService;
        _chatService.OnNewMessage += HandleNewMessage;
        _chatService.OnNewUser += HandleNewUser;
        _messageService = messageService;
        _navigatorService = navigatorService;
    }

    public override async Task Start()
    {
        var recieve = _chatService.ReadyQueue();
        var listen=  _networkService.HandleIncomingMessages();
        var chatMessage = new ChatMessage{ChatType = "JOIN", chatMessage = "", GroupCode = _chatModel.GroupId};
        await _networkService.SendMessage(chatMessage);
        await Task.WhenAll(listen, recieve);
    }

    private void OnFormClosed(object? sender, EventArgs e)
    {
        _ = _chatService.CloseConnection();
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
        var chatMessage = new ChatMessage{ChatType = "CHAT", chatMessage = message, GroupCode = _chatModel.GroupId};
        _chatService.HandleUserInput(chatMessage);
    }
}