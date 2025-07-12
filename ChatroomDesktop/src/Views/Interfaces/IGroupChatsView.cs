using ChatroomDesktop.Models;
using ChatroomDesktop.Models.EventArgs;

namespace ChatroomDesktop.Views;

public interface IGroupChatsView
{
    event EventHandler?  CreateGroupClicked;
    event EventHandler? JoinGroupClicked;
    
    event EventHandler<GroupButtonEventArgs>? GroupButtonClicked;

    void UpdateButtons(List<GroupModel> groups);
}