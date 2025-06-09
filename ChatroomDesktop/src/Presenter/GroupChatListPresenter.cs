using ChatroomDesktop.Services;
using ChatroomDesktop.Views;

namespace ChatroomDesktop.Presenter;

public class GroupChatListPresenter
{
    private readonly ChatService _chatService;
    private readonly NetworkService _networkService;
    private readonly IGroupChatsView _view;

    public GroupChatListPresenter(IGroupChatsView view, NetworkService networkService, ChatService chatService)
    {
        _view = view;
        _networkService = networkService;
        _chatService = chatService;
    }
}