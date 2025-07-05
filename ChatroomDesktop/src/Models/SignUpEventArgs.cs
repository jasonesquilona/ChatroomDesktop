namespace ChatroomDesktop.Models;

public class SignUpEventArgs : EventArgs{
    public bool isSignupSuccess { get;}
    
    public ConnectMessage msg { get;}

    public SignUpEventArgs(bool isSignupSuccess,ConnectMessage msg)
    {
        this.isSignupSuccess = isSignupSuccess;
        this.msg = msg;
    }
    
}