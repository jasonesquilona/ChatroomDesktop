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
        _view.JoinGroupClicked += OnJoinGroupClicked;
    }

    private async void OnCreateGroupClicked(object? sender, EventArgs e)
    {
        string groupName = "";
        GroupCreationForm formDialog= new GroupCreationForm();
        if (formDialog.ShowDialog() == DialogResult.OK)
        {
            groupName = formDialog.GroupNameEntered;
            await SendCreateGroupData(groupName);
            Console.WriteLine(groupName);
        }
        formDialog.Dispose();
    }

    private async void OnJoinGroupClicked(object? sender, EventArgs e)
    {
        string groupCode = "";
        GroupCreationForm formDialog= new GroupCreationForm();
        formDialog.ChangeLabelText("Enter Group Code");
        formDialog.ChangeTextLength(5);
        if (formDialog.ShowDialog() == DialogResult.OK)
        {
            groupCode = formDialog.GroupNameEntered;
        }
        else
        {
            
        }
        formDialog.Dispose();
    }

    private async Task SendCreateGroupData(string groupName)
    {
        var message = new CreateGroupMessage();
        message.groupName = groupName;
        
        await _networkService.SendGroupCreationRequest(message);
        
    }
}