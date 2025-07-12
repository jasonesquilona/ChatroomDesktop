namespace ChatroomDesktop.Models;

public class UserModel
{
    public string Username { get; set; }

    public List<GroupModel> Groups { get; set; }
    
    public int UserId { get; set; }

    public void AddNewGroup(ConfirmGroupJoinMessage? groupInfo)
    {
        var group = new GroupModel{GroupName = groupInfo?.GroupName, GroupId = groupInfo.GroupCode};
        Groups.Add(group);
    }
}