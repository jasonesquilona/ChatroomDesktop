using ChatroomDesktop.Models;
using ChatroomDesktop.Models.EventArgs;
using ChatroomDesktop.Services;
using ChatroomDesktop.Services.Interfaces;
using ChatroomDesktop.Views;
using Message = ChatroomDesktop.Models.Message;

namespace ChatroomDesktop.Presenter;

public class GroupChatListPresenter : BasePresenter<IGroupChatsView>
{
    private readonly IChatService _chatService;
    private readonly INetworkService _networkService;
    private readonly IGroupChatsView _view;
    private readonly UserModel _user;
    private readonly INavigatorService _navigatorService;
    private readonly IMessageService _messageService;
    private bool _isConnected;

    public GroupChatListPresenter(IGroupChatsView view, INetworkService networkService, IChatService chatService,
        UserModel user, INavigatorService navigatorService, IMessageService messageService) : base(view)
    {
        _view = view;
        _networkService = networkService;
        _chatService = chatService;
        _navigatorService = navigatorService;
        _messageService = messageService;
        _view.CreateGroupClicked += OnCreateGroupClicked;
        _view.JoinGroupClicked += OnJoinGroupClicked;
        _view.GroupButtonClicked += OnButtonClick;
        _user = user;
        _isConnected = true;
        _chatService = chatService;
        _view.UpdateButtons(user.Groups);
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
        Console.WriteLine("Join Group Form Entered");
        GroupCreationForm formDialog= new GroupCreationForm();
        formDialog.ChangeLabelText("Enter Group Code");
        formDialog.ChangeTextLength(5);
        try
        {
            if (formDialog.ShowDialog() == DialogResult.OK)
            {
                groupCode = formDialog.GroupNameEntered;
                if (!CheckGroup(groupCode))
                {
                    var groupInfo = await _networkService.SendJoinGroupRequest(_user, groupCode);
                    if (groupInfo != null)
                    {
                        Console.WriteLine($"joined {groupCode}");
                        _user.AddNewGroup(groupInfo);
                        _view.UpdateButtons(_user.Groups);
                    }
                    else
                    {
                        _messageService.ShowMessage("Error Joining Group, Try Again");
                    }
                    formDialog.Dispose();
                }
                else
                {
                    _messageService.ShowMessage("You are already apart of this group");
                }
            }
            else
            {

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        formDialog.Dispose();
    }

    private void OnButtonClick(object? sender, GroupButtonEventArgs e)
    {
        _ = HandleGroupButtonClick(e.GroupName, e.GroupId);
    }

    private async Task SendCreateGroupData(string groupName)
    {
        var message = new CreateGroupMessage();
        message.groupName = groupName;
        
        var success = await _networkService.SendGroupCreationRequest(message);
        if (success != null)
        {
            _user.AddNewGroup(success);
            _view.UpdateButtons(_user.Groups);
        }
        else
        {
            _messageService.ShowMessage("Group Creation Failed");
        }
        
    }

    private async Task HandleGroupButtonClick(string groupName, string groupId)
    {
        var result = await _networkService.ConnectToGroupChat(groupName, groupId, _user);
        if (result)
        {
            await HandleNewChat(groupId);
            _view.HideForm();
        }
    }

    private async Task HandleNewChat(string groupId)
    {
        await _navigatorService.OpenChatroomPage(_chatService, _networkService, _user, _navigatorService, _messageService, groupId);
    }

    private bool CheckGroup(string groupCode)
    {
        foreach (var group in _user.Groups)
        {
            if (group.GroupId == groupCode)
            {
                return true;
            }
        }
        return false;
    }
}