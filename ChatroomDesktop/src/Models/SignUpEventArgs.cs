namespace ChatroomDesktop.Models;

public class SignUpEventArgs : EventArgs{
    public bool isSignupSuccess { get;}
    
    public string name { get;}

    public SignUpEventArgs(bool isSignupSuccess, string name)
    {
        this.isSignupSuccess = isSignupSuccess;
        this.name = name;
    }
    
}