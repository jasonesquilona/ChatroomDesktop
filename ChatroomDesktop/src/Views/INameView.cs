namespace ChatroomDesktop.Views;

public interface INameView
{
    string Name { get; }
    string Password { get; }
    event EventHandler?  EnterClicked;
    void CloseForm();
    event EventHandler? SignUpClicked;
    event EventHandler? FormClosed;
}