namespace ChatroomDesktop.Views;

public partial class NameForm : Form, INameView
{
    public NameForm()
    {
        InitializeComponent();
        enterBtn.Click += (s,e) => EnterClicked?.Invoke(this, EventArgs.Empty);
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
}