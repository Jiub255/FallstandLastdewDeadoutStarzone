using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Not sure if this is possible. It'd be nice though. 
public class MBAutoUnsubscribe : MonoBehaviour
{
/*    private class Subscription
    {
        public Action Action { get; set; }
        public Action Method { get; set; }

        public Subscription(Action action, Action method)
        {
            Action = action;
            Method = method;
        }
    }

    private List<Subscription> _subscriptions = new List<Subscription>();

    protected void Subscribe(Action action, Action method)
    {
        action += method;
        _subscriptions.Add(new Subscription(action, method));
    }

    private void OnDisable()
    {
        foreach (Subscription subscription in _subscriptions)
        {
            subscription.Action -= subscription.Method;
        }
    }*/
}