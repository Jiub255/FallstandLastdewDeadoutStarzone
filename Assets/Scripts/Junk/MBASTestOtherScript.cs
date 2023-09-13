using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MBASTestOtherScript : MBAutoUnsubscribe
{
    private static Action _onTest;
    public static void OnTest(bool subscribe, Action handler)
    {
        if (subscribe)
        {
            _onTest += handler;
        }
        else
        {
            _onTest -= handler;
        }
    }

    public static void MakeAction(Action action)
    {
        
    }

    private void Start()
    {
        _onTest?.Invoke();
    }

/*    public static event Action OnTest
    {
        add
        {
            lock (typeof(MBASTestOtherScript))
            {
                _onTest += value;
            }
        }
        remove
        {
            lock (typeof(MBASTestOtherScript))
            {
                _onTest -= value;
            }
        }
    }*/
}