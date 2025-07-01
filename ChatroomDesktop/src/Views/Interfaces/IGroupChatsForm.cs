using ChatroomDesktop.Models;

namespace ChatroomDesktop.Views;

public interface IGroupChatsView
{
    event EventHandler?  CreateGroupClicked;
    event EventHandler? JoinGroupClicked;
}