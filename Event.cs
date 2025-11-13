using System.Collections.Generic;
using System.Collections.ObjectModel;


public delegate void Listener<T>(params T[] args);


public class Event<T>
{
    private List<Listener<T>> m_listeners;
    public ReadOnlyCollection<Listener<T>> Listeners;


    public Event()
    {
        m_listeners = new();
        Listeners = new(m_listeners);
    }


    public Event(List<Listener<T>> listeners)
    {
        m_listeners = new(listeners);
        Listeners = new(m_listeners);
    }


    public virtual void Add(Listener<T> listener) { m_listeners.Add(listener); }


    public virtual void Remove(Listener<T> listener) { m_listeners.Remove(listener); }


    public void Invoke(params T[] args)
    {
        foreach (Listener<T> listener in m_listeners)
        {
            listener.Invoke(args);
        }
    }
}