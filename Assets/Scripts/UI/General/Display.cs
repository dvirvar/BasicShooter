using UnityEngine;

/// <summary>
/// Represent the Info
/// </summary>
/// <typeparam name="T">The Info</typeparam>
public abstract class Display<T> : MonoBehaviour
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
