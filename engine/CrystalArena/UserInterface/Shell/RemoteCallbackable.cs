using System;

namespace CrystalArena.UserInterface.Shell;

public abstract class RemoteCallbackable
{
    public Action<string> OnCallback;
    private static readonly Random _random = new Random();
    
    public RemoteCallbackable()
    {
        CallbackId = _random.Next().ToString();
    }
    
    public void RegisterCallback(Action<string> callback)
    {
        OnCallback = callback;
    }

    public string CallbackId
    {
        get;
        set;
    }
    
    public void Callback(string result)
    {
        OnCallback?.Invoke(result);
    }

    public virtual object ToJson()
    {
        return null;
    }
}