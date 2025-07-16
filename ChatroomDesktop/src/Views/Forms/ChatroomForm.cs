using ChatroomServer.Models;

namespace ChatroomDesktop.Views;

public partial class ChatroomForm : Form, IChatroomView
{
    public ChatroomForm()
    {
        InitializeComponent();
        sendButton.Click += (s,e) => EnterClicked?.Invoke(this, EventArgs.Empty);
    }
    
    public string CurrentMessage => sendMsgTextbox.Text;
    
    public event EventHandler? EnterClicked;
    public event EventHandler? FormClosed;

    public void AddMessage(string message)
    {
        chatText.Text += message+ Environment.NewLine;
    }

    public void AddNewUser(List<UserDetails> usernames)
    {
        if (memberList.Text.Length != 0) {
            memberList.Clear();
        }
        
        foreach (var user in usernames) {
           memberList.Text += user.Username + Environment.NewLine; 
        }
    }

    public void ClearMessageBox()
    {
        sendMsgTextbox.Clear();
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        FormClosed?.Invoke(this, EventArgs.Empty);
        base.OnFormClosing(e);
        
    }
    
    
}