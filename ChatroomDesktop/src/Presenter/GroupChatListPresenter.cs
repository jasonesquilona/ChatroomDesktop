using ChatroomDesktop.Models;
using ChatroomDesktop.Services;
using ChatroomDesktop.Views;
using Message = ChatroomDesktop.Models.Message;

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
        _view.CreateGroupClicked += OnCreateGroupClicked;
    }

    private void OnCreateGroupClicked(object? sender, EventArgs e)
    {
        string groupName = "";
        GroupCreationForm formDialog= new GroupCreationForm();
        if (formDialog.ShowDialog() == DialogResult.OK)
        {
            groupName = formDialog.GroupNameEntered;
        }
        else
        {
            
        }
        formDialog.Dispose();
        Console.WriteLine(groupName);
    }

    private void SendCreateGroupData(string groupName)
    {
        var message = new CreateGroupMessage();
        message.MessageType = "CREATEGROUP";
        message.groupName = groupName;
    }
}