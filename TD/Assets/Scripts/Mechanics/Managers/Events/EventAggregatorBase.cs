#define WithDelay
/*
 * @author = Shustrik 
 * 06.08.2016
*/

#if WithDelay
using System.Collections;
using UnityEngine;
#endif

public class EventAggregatorBase
{
    #region Delegate Types

    public delegate void CustomEventDelegate();
    public delegate void CustomEventDelegate<T>(T arg);
    public delegate void CustomEventDelegate<T1, T2>(T1 arg1, T2 arg2);
    public delegate void CustomEventDelegate<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3);
    public delegate void CustomEventDelegate<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);

    #endregion

    #region Subscribe

    public static void SubscribeForEvent(CustomEventDelegate eventName, CustomEventDelegate callback)
    {
        eventName += callback;
    }
    public static void SubscribeForEvent<T>(CustomEventDelegate<T> eventName, CustomEventDelegate<T> callback)
    {
        eventName += callback;
    }
    public static void SubscribeForEvent<T1, T2>(CustomEventDelegate<T1, T2> eventName, CustomEventDelegate<T1, T2> callback)
    {
        eventName += callback;
    }
    public static void SubscribeForEvent<T1, T2, T3>(CustomEventDelegate<T1, T2, T3> eventName, CustomEventDelegate<T1, T2, T3> callback)
    {
        eventName += callback;
    }

    public static void SubscribeForEvent<T1, T2, T3, T4>(CustomEventDelegate<T1, T2, T3, T4> eventName, CustomEventDelegate<T1, T2, T3, T4> callback)
    {
        eventName += callback;
    }

    #endregion

    #region Unsubscribe

    public static void UnsubscribeFromEvent(CustomEventDelegate eventName, CustomEventDelegate callback)
    {
        eventName -= callback;
    }
    public static void UnsubscribeFromEvent<T>(CustomEventDelegate<T> eventName, CustomEventDelegate<T> callback)
    {
        eventName -= callback;
    }
    public static void UnsubscribeFromEvent<T1, T2>(CustomEventDelegate<T1, T2> eventName, CustomEventDelegate<T1, T2> callback)
    {
        eventName -= callback;
    }
    public static void UnsubscribeFromEvent<T1, T2, T3>(CustomEventDelegate<T1, T2, T3> eventName, CustomEventDelegate<T1, T2, T3> callback)
    {
        eventName -= callback;
    }

    public static void UnsubscribeFromEvent<T1, T2, T3, T4>(CustomEventDelegate<T1, T2, T3, T4> eventName, CustomEventDelegate<T1, T2, T3, T4> callback)
    {
        eventName -= callback;
    }

    #endregion

    #region Publish

    public static void Publish(CustomEventDelegate eventName)
    {
        eventName.Invoke();
    }

    public static void Publish<T>(CustomEventDelegate<T> eventName, T arg)
    {
        eventName.Invoke(arg);
    }

    public static void Publish<T1, T2>(CustomEventDelegate<T1, T2> eventName, T1 arg1, T2 arg2)
    {
        eventName.Invoke(arg1, arg2);
    }

    public static void Publish<T1, T2, T3>(CustomEventDelegate<T1, T2, T3> eventName, T1 arg1, T2 arg2, T3 arg3)
    {
        eventName.Invoke(arg1, arg2, arg3);
    }

    public static void Publish<T1, T2, T3, T4>(CustomEventDelegate<T1, T2, T3, T4> eventName, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        eventName.Invoke(arg1, arg2, arg3, arg4);
    }

    #endregion

    #region Publish With Delay

#if WithDelay

    private static MonoBehaviour delayHelperObject;
    public static void SetDelayObject(MonoBehaviour _delayHelperObject)
    {
        delayHelperObject = _delayHelperObject;
    }

#endif

    public static void PublishWithDelay(float delay, CustomEventDelegate eventName)
    {
        MakeDelay(delay, () =>
        {
            eventName.Invoke();
        });
    }

    public static void PublishWithDelay<T>(float delay, CustomEventDelegate<T> eventName, T arg)
    {
        MakeDelay(delay, () =>
        {
            eventName.Invoke(arg);
        });
    }

    public static void PublishWithDelay<T1, T2>(float delay, CustomEventDelegate<T1, T2> eventName, T1 arg1, T2 arg2)
    {
        MakeDelay(delay, () =>
        {
            eventName.Invoke(arg1, arg2);
        });
    }

    public static void PublishWithDelay<T1, T2, T3>(float delay, CustomEventDelegate<T1, T2, T3> eventName, T1 arg1, T2 arg2, T3 arg3)
    {
        MakeDelay(delay, () =>
        {
            eventName.Invoke(arg1, arg2, arg3);
        });
    }

    public static void PublishWithDelay<T1, T2, T3, T4>(float delay, CustomEventDelegate<T1, T2, T3, T4> eventName, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        MakeDelay(delay, () =>
        {
            eventName.Invoke(arg1, arg2, arg3, arg4);
        });
    }

    private static void MakeDelay(float delay, CustomEventDelegate callback)
    {
#if WithDelay
        if (delayHelperObject != null)
            delayHelperObject.StartCoroutine(DelayedInvoke(delay, callback));
        else
#endif
            callback.Invoke();
    }

#if WithDelay
    private static IEnumerator DelayedInvoke(float delay, CustomEventDelegate callback)
    {
        yield return new WaitForSeconds(delay);
        callback.Invoke();
    }
#endif

#endregion
}

