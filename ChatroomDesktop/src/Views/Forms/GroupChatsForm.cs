using ChatroomDesktop.Presenter;

namespace ChatroomDesktop.Views;

public partial class GroupChatsForm : Form, IGroupChatsView
{   
    public event EventHandler? CreateGroupClicked;
    public event EventHandler? JoinGroupClicked;
    public event EventHandler? FormClosed;

    private GroupChatListPresenter _groupChatListPresenter;
    public GroupChatsForm()
    {
        InitializeComponent();
        createGroupButton.Click += (s,e) => CreateGroupClicked?.Invoke(this, EventArgs.Empty);
        JoinGroupButton.Click += (s,e) => JoinGroupClicked?.Invoke(this, EventArgs.Empty);
        
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
    
    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        FormClosed?.Invoke(this, EventArgs.Empty); 
        base.OnFormClosing(e);
        
    }
    
    public void CloseForm()
    {
        this.Close();
    }

    public void SetPresenter(GroupChatListPresenter groupListPrsenter)
    {
        _groupChatListPresenter = groupListPrsenter;
    }
}