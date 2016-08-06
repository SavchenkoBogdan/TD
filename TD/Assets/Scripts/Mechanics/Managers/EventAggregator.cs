using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class EventAggregator
{    
    public static CustomEvent UnitReachedFinish = new CustomEvent();
    public static CustomEvent EnergyRecieved = new CustomEvent();
}

public class CustomEvent
{
    private readonly  List<Action> _callbacks = new List<Action>();

    public void Subscribe(Action callback)
    {
        _callbacks.Add(callback);
    }

    public void Publish()
    {
        foreach (var callback in _callbacks)
        {
            callback();
        }
    }
}

