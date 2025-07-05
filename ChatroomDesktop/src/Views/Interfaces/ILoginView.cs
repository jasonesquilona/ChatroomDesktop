namespace ChatroomDesktop.Views;

public interface ILoginView
{
    string Name { get; }
    string Password { get; }
    event EventHandler?  EnterClicked;
    void CloseForm();
    event EventHandler? SignUpClicked;
    event EventHandler? FormClosed;

    void IncorrectLoginDetails();

    void ShowForm();
    
    void HideForm();
}