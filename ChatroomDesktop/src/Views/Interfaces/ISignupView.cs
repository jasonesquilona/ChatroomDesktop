namespace ChatroomDesktop.Views;

public interface ISignupView
{
    string Username{ get; }
    string Password{ get; }
    
    event EventHandler?  EnterClicked;
    
    event EventHandler? CancelClicked;
    
    void CloseForm();
    
    void HideForm();
}