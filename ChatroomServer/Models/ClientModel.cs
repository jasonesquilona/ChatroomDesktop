using System.Net.Sockets;

namespace ChatServer;

public class ClientModel
{

    private TcpClient clientID;
    
    public TcpClient ClientID
    {
        get { return this.clientID; } 
        set{} 
    }

    private string name;
    
    public string Name
    {
        get { return this.name;} 
        set{}
    }
  

    public ClientModel(TcpClient tcpClient, string name)
    {
        this.clientID = tcpClient;
        this.name = name;
    }
    
    
}

