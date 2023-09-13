using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Not sure if this is possible. It'd be nice though. 
public class MBAutoUnsubscribe : MonoBehaviour
{
    private class Subscription
    {
        public Action<bool, Action> Del { get; set; }
        public Action Method { get; set; }

        public Subscription(Action<bool, Action> del, Action method)
        {
            Del = del;
            Method = method;
        }
    }

    private List<Subscription> _subscriptions = new List<Subscription>();

    protected void Subscribe(Action<bool, Action> eventDelegate, Action handler)
    {
        eventDelegate(true, handler);
        _subscriptions.Add(new Subscription(eventDelegate, handler));
    }

    private void OnDisable()
    {
        foreach (Subscription subscription in _subscriptions)
        {
            subscription.Del(false, subscription.Method);
        }
    }
/*    private class Subscription
    {
        public Delegate Del { get; set; }
        public Delegate Method { get; set; }

        public Subscription(Delegate del, Delegate method)
        {
            Del = del;
            Method = method;
        }
    }

    private List<Subscription> _subscriptions = new List<Subscription>();

    protected void Subscribe(ref Delegate eventDelegate, Delegate handler)
    {
        Delegate.Combine(eventDelegate, handler);
//        eventDelegate += handler;
        _subscriptions.Add(new Subscription(eventDelegate, handler));
    }

    private void OnDisable()
    {
        foreach (Subscription subscription in _subscriptions)
        {
            Delegate.Remove(subscription.Del, subscription.Method);
//            subscription.Del -= subscription.Method;
        }
    }*/

/*    Wire(ref myObject.SomeEventDelegate, MyHandler);

    private void Wire(ref Delegate eventDelegate, Delegate handler)
    {
        // Pre validate the subscription.
        eventDelegate += handler;
        // Post actions (storing subscribed event handlers in a list)
    }*/
}