namespace ChatroomDesktop.Views;

public partial class SignupForm : Form, ISignupView
{
    public SignupForm()
    {
        InitializeComponent();
        confirmButton.Click += (s,e) => EnterClicked?.Invoke(this, EventArgs.Empty);
        cancelButton.Click += (sender, args) =>  CancelClicked?.Invoke(this, EventArgs.Empty);
    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {
    }

    private void label1_Click(object sender, EventArgs e)
    {
    }

    private void label2_Click(object sender, EventArgs e)
    {
    }

    public string Username => nameText.Text;
    public string Password => passwordText.Text;
    public event EventHandler? EnterClicked;
    public event EventHandler? CancelClicked;

    public void CloseForm()
    {
        this.Close();
    }

    public void HideForm()
    {
        this.Hide();
    }
}