namespace ChatroomDesktop.Views;

public interface IChatroomView
{
    string CurrentMessage { get; }
    event EventHandler?  EnterClicked;
    event EventHandler? FormClosed;
    void AddMessage(string message);
    void AddNewUser(string[] username);
    void ClearMessageBox();
}