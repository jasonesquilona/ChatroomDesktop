namespace ChatroomDesktop.Models;

public class Message
{
    public string MessageType { get; set; }
    public string Sender {get; set;}
    public string ChatMessage {get; set;}
    
    public string[] UserList {get; set;}
}