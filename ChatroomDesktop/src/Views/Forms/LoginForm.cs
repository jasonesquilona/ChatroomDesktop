using ChatroomDesktop.Presenter;

namespace ChatroomDesktop.Views;

public partial class LoginForm : Form, ILoginView
{
    
    public event EventHandler? SignUpClicked;
    public event EventHandler? FormClosed;
    
    private LoginPresenter _presenter;
    public LoginForm()
    {
        InitializeComponent();
        enterBtn.Click += (s,e) => EnterClicked?.Invoke(this, EventArgs.Empty);
        signupButton.Click += (s,e) => SignUpClicked?.Invoke(this, EventArgs.Empty);
        this.Shown += NameForm_Shown;
    }

    public string Name => txtName.Text;
    public string Password => txtPassword.Text;
    public event EventHandler? EnterClicked;

    private void enterBtn_Click(object sender, EventArgs e)
    {
    }

    private void txtName_TextChanged(object sender, EventArgs e)
    {
    }

    private async void NameForm_Shown(object? sender, EventArgs e)
    {
        await Task.Delay(100);
        
        try
        {
            await _presenter.SetupConnection();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to connect: {ex.Message}");
        }
        
    }
    
    public void SetPresenter(LoginPresenter presenter)
    {
        _presenter = presenter;
    }

    public void CloseForm()
    {
        this.Close();
    }

    public void ShowForm()
    {
        this.Show();
    }

    public void HideForm()
    {
        this.Hide();
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

    public void IncorrectLoginDetails()
    {
        incorrectLabel.Visible = true;
    }

    private void label3_Click(object sender, EventArgs e)
    {
        throw new System.NotImplementedException();
    }
}