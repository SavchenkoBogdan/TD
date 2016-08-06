using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class CustomEvent<T>:ICustomEvent
{
    //public delegate void CustomEventDelegate();
    //public delegate void CustomEventDelegate<T>(T arg);
    //public delegate void CustomEventDelegate<T1, T2>(T1 arg1, T2 arg2);
    //public delegate void CustomEventDelegate<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3);
    //public delegate void CustomEventDelegate<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);

    //private List<CustomEventDelegate> callbacks = new List<CustomEventDelegate>();
    //public void Subscribe(CustomEventDelegate<T> callback)
    //{
    //    callbacks.Add(callback);
    //}

    //public void Publish()
    //{
    //    foreach (var callback in callbacks)
    //    {
    //        callback.Invoke();
    //    }
    //}
}

