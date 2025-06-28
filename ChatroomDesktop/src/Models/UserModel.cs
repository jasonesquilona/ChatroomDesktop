namespace ChatroomDesktop.Models;

public class UserModel
{
    public string Username { get; set; }

    public List<GroupModel> Groups { get; set; }
    
    public int UserId { get; set; }
}