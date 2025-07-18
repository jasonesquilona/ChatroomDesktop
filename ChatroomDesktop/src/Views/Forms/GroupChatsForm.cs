using System.Text.RegularExpressions;
using ChatroomDesktop.Models;
using ChatroomDesktop.Models.EventArgs;
using ChatroomDesktop.Presenter;
using ChatroomDesktop.Services;
using ChatroomDesktop.Services.Interfaces;
using Microsoft.VisualBasic.ApplicationServices;

namespace ChatroomDesktop.Views;

public partial class GroupChatsForm : Form, IGroupChatsView
{   
    public event EventHandler? CreateGroupClicked;
    public event EventHandler? JoinGroupClicked;
    public event EventHandler<GroupButtonEventArgs>? GroupButtonClicked;

    private readonly IChatService _chatService;

    private GroupChatListPresenter _groupChatListPresenter;
    public GroupChatsForm(IChatService service)
    {
        InitializeComponent();
        createGroupButton.Click += (s,e) => CreateGroupClicked?.Invoke(this, EventArgs.Empty);
        JoinGroupButton.Click += (s,e) => JoinGroupClicked?.Invoke(this, EventArgs.Empty);
        _chatService = service;
        
        this.FormClosed += OnMainFormClosed;
    }

    private void button1_Click(object sender, EventArgs e)
    {
    }

    private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
    {
    }

    private void button1_Click_1(object sender, EventArgs e)
    {
    }

    public void SetPresenter(GroupChatListPresenter groupListPrsenter)
    {
        _groupChatListPresenter = groupListPrsenter;
    }
    
    private async void OnMainFormClosed(object? sender, EventArgs e)
    {
        Console.WriteLine("Closing Form");
        await _chatService.CloseConnection();
        Application.Exit();
    }
    
    public void UpdateButtons(List<GroupModel> groups)
    {
        groupChatList.Controls.Clear();
        if (groups.Count > 0)
        {
            foreach (var group in groups)
            {
                var button = new Button();
                button.Text = group.GroupName;
                button.AutoSize = true;
                button.Tag = group.GroupId;
                button.Dock = DockStyle.Fill;

                var groupChatEventArgs = new GroupButtonEventArgs();
                groupChatEventArgs.GroupName = group.GroupName;
                groupChatEventArgs.GroupId = group.GroupId;
                button.Click += (s, e) => GroupButtonClicked?.Invoke(this, groupChatEventArgs);
                groupChatList.Controls.Add(button);
            }
        }
    }
    public void HideForm()
    {
        this.Hide();
    }
}