using ChatroomDesktop.Services.Interfaces;

namespace ChatroomDesktop.Services;

public class MessageService :IMessageService
{
    public void ShowMessage(string message)
    {
        MessageBox.Show("Wrong username or password!");
    }
}