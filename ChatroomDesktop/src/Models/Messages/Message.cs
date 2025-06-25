using System.Text.Json.Serialization;

namespace ChatroomDesktop.Models;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "MessageType")]
[JsonDerivedType(typeof(LoginMessage), "LOGIN")]
[JsonDerivedType(typeof(ChatMessage), "CHAT")]
[JsonDerivedType(typeof(CreateGroupMessage), "CREATEGROUP")]
[JsonDerivedType(typeof(SignupMessage), "SIGNUP")]
[JsonDerivedType(typeof(ConnectMessage), "CONNECT")]
[JsonDerivedType(typeof(ResponseMessage), "RESPONSE")]
public abstract class Message
{
}

public class LoginMessage : Message
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
}

public class ConnectMessage : Message
{
    public string User {get; set;}
}

public class ResponseMessage:Message
{
    public string Response {get; set;}
}