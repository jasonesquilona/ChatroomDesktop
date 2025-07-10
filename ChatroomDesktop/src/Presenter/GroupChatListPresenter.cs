using ChatroomDesktop.Models;
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
    private UserModel _user;
    private bool _isConnected;

    public GroupChatListPresenter(IGroupChatsView view, INetworkService networkService, IChatService chatService, UserModel user) : base(view)
    {
        _view = view;
        _networkService = networkService;
        _chatService = chatService;
        _view.CreateGroupClicked += OnCreateGroupClicked;
        _view.JoinGroupClicked += OnJoinGroupClicked;
        _view.GroupButtonClicked += OnButtonClick;
        _user = user;
        _isConnected = true;
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
        GroupCreationForm formDialog= new GroupCreationForm();
        formDialog.ChangeLabelText("Enter Group Code");
        formDialog.ChangeTextLength(5);
        try
        {
            if (formDialog.ShowDialog() == DialogResult.OK)
            {
                if (CheckGroup(groupCode))
                {
                    groupCode = formDialog.GroupNameEntered;
                    string groupName = await _networkService.SendJoinGroupRequest(_user, groupCode);
                    Console.WriteLine($"joined {groupCode}");
                    formDialog.Dispose();
                }
                else
                {
                    MessageBox.Show("You are already apart of this group");
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

    private async void OnButtonClick(object? sender, EventArgs e)
    {
        
    }

    private async Task SendCreateGroupData(string groupName)
    {
        var message = new CreateGroupMessage();
        message.groupName = groupName;
        
        await _networkService.SendGroupCreationRequest(message);
        
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