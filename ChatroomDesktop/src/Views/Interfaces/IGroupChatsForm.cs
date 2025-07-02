using ChatroomDesktop.Models;

namespace ChatroomDesktop.Views;

public interface IGroupChatsView
{
    event EventHandler?  CreateGroupClicked;
    event EventHandler? JoinGroupClicked;
    
    event EventHandler? GroupButtonClicked;

    void UpdateButtons(List<GroupModel> groups);
}