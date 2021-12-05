
/// <summary>
/// Represent the Info, But also is a pooled object
/// </summary>
/// <typeparam name="T">The Info</typeparam>
public abstract class PooledDisplay<T> : PoolObject
{
    public T info
    {
        get
        {
            return _info;
        }
        set
        {
            _info = value;
            setView(value);
        }
    }
    private T _info;

    public void refreshDisplay()
    {
        setView(info);
    }

    protected abstract void setView(T info);
}
