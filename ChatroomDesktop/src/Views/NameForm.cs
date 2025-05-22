namespace ChatroomDesktop.Views;

public partial class NameForm : Form, INameView
{
    
    public event EventHandler? SignUpClicked;
    public event EventHandler? FormClosed;
    public NameForm()
    {
        InitializeComponent();
        enterBtn.Click += (s,e) => EnterClicked?.Invoke(this, EventArgs.Empty);
        signupButton.Click += (s,e) => SignUpClicked?.Invoke(this, EventArgs.Empty);
    }

    public string Name => txtName.Text;
    public event EventHandler? EnterClicked;

    private void enterBtn_Click(object sender, EventArgs e)
    {
    }

    private void txtName_TextChanged(object sender, EventArgs e)
    {
    }

    public void CloseForm()
    {
        this.Close();
    }

    private void label1_Click(object sender, EventArgs e)
    {
    }

    private void label2_Click(object sender, EventArgs e)
    {
    }
    
    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        FormClosed?.Invoke(this, EventArgs.Empty);
        base.OnFormClosing(e);
    }
}