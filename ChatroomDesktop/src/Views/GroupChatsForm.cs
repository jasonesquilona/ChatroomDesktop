namespace ChatroomDesktop.Views;

public partial class GroupChatsForm : Form, IGroupChatsView
{   
    public event EventHandler? CreateGroupClicked;
    public GroupChatsForm()
    {
        InitializeComponent();
        createGroupButton.Click += (s,e) => CreateGroupClicked?.Invoke(this, EventArgs.Empty);
    }

    private void button1_Click(object sender, EventArgs e)
    {
    }

    private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
    {
    }

    private void button1_Click_1(object sender, EventArgs e)
    {
        throw new System.NotImplementedException();
    }
}