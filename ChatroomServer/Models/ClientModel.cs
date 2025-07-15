using System.Net.Sockets;
using ChatroomServer.Models;

namespace ChatServer;

public class ClientModel
{

    private TcpClient clientID;
    
    public TcpClient ClientID
    {
        get { return this.clientID; } 
        set{} 
    }

    private UserDetails details;
    
    public UserDetails Details
    {
        get { return this.details; ;} 
        set{}
    }

    public ClientModel(TcpClient tcpClient, UserDetails details)
    {
        this.clientID = tcpClient;
        this.details = details;
    }
    
    
}

