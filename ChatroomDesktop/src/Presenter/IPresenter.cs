namespace ChatroomDesktop.Presenter;

public interface IPresenter
{
    Task Start();
    void Destroy();
}