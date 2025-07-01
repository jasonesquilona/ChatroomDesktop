using System.Text.Json.Serialization;

namespace ChatroomDesktop.Models;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "MessageType")]
[JsonDerivedType(typeof(LoginRequestMessage), "LOGIN")]
[JsonDerivedType(typeof(ChatMessage), "CHAT")]
[JsonDerivedType(typeof(CreateGroupMessage), "CREATEGROUP")]
[JsonDerivedType(typeof(SignupMessage), "SIGNUP")]
[JsonDerivedType(typeof(ConnectMessage), "CONNECT")]
[JsonDerivedType(typeof(JoinGroupMessage), "JOINGROUP")]
[JsonDerivedType(typeof(ConfirmGroupJoinMessage), "CONFIRMJOIN")]
public abstract class Message
{
}

public class LoginRequestMessage : Message
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class SignupMessage :Message
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class CreateGroupMessage : Message
{
    public string groupName { get; set; }
}
public class ChatMessage : Message
{
    public string Sender {get; set;}
    
    public string ChatType {get; set;}
    public string chatMessage {get; set;}
    
    public string[] UserList {get; set;}
    
    public string GroupCode {get; set;}
}

public class ConnectMessage : Message
{
    public string Response { get; set; }
    public int Userid {get; set;}
    public string Username {get; set;}
    public List<GroupModel> GroupList {get; set;}
} 

public class JoinGroupMessage : Message
{
    public string GroupCode {get; set;}
    public int UserId {get; set;}
}

public class ConfirmGroupJoinMessage : Message
{
    public string Response { get; set; }
    public string GroupCode {get; set;}
    public int UserId {get; set;}
    public string GroupName {get; set;}
}