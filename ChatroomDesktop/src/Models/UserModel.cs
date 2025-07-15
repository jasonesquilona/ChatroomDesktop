using ChatroomServer.Models;

namespace ChatroomDesktop.Models;

public class UserModel
{

    public UserDetails Details { get; set; }
    public List<GroupModel> Groups { get; set; }
    

    public void AddNewGroup(ConfirmGroupJoinMessage? groupInfo)
    {
        var group = new GroupModel{GroupName = groupInfo?.GroupName, GroupId = groupInfo.GroupCode};
        Groups.Add(group);
    }
}