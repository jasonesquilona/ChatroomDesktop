namespace ChatroomDesktop.Presenter;

//Abstract class for all the Presenters in this App
//Shared Logic of Destroy between presenters 

public abstract class BasePresenter<TView> : IPresenter where TView : class
{
    protected TView View;

    protected BasePresenter(TView view)
    {
        this.View = view;
    }

    public virtual async Task Start()
    {
    }

    public virtual void Destroy()
    {
        View = null;
    }
}