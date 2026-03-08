using System.Collections.Generic;
using System.Collections.ObjectModel;


public delegate void Listener<T>(T arg);


public class GameEvent<T>
{
    private List<Listener<T>> m_listeners;
    public ReadOnlyCollection<Listener<T>> Listeners;

    private List<Listener<T>> m_listenersRemovedAfterInvoke;


    public GameEvent()
    {
        m_listeners = new();
        m_listenersRemovedAfterInvoke = new();
        Listeners = new(m_listeners);
    }


    public GameEvent(List<Listener<T>> listeners)
    {
        m_listeners = new(listeners);
        Listeners = new(m_listeners);
    }


    public virtual void Add(Listener<T> listener, bool removeAfterInvoke = false)
    {
        m_listeners.Add(listener);
        
        if (removeAfterInvoke)
            m_listenersRemovedAfterInvoke.Add(listener);
    }


    public virtual void Remove(Listener<T> listener) { m_listeners.Remove(listener); }


    public virtual void RemoveAll() { m_listeners.Clear(); }


    public void Invoke(T arg = default)
    {
        foreach (Listener<T> listener in m_listeners)
        {
            listener.Invoke(arg);
        }

        // Remove all listeners which are to-be-removed.
        m_listenersRemovedAfterInvoke.ForEach((Listener<T> listener) =>
        {
            m_listeners.Remove(listener);
        });
    }
}