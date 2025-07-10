namespace ChatroomDesktop.Presenter;

//Abstract class for all the Presenters in this App
//Shared Logic of Destroy between presenters 

public abstract class BasePresenter<TView> : IPresenter where TView : class
{
    protected TView view;

    protected BasePresenter(TView view)
    {
        this.view = view;
    }

    public virtual void Start()
    {
    }

    public virtual void Destroy()
    {
        view = null;
    }
}