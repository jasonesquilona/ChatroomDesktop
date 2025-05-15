namespace ChatroomDesktop.Views;

public interface INameView
{
    string Name { get; }
    event EventHandler?  EnterClicked;
    void CloseForm();
}