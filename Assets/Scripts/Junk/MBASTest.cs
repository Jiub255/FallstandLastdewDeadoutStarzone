using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MBASTest : MBAutoUnsubscribe
{

    private void OnEnable()
    {
        Subscribe(MBASTestOtherScript.OnTest, TestMethod);
    }

    public void TestMethod()
    {
        Debug.Log("Test worked. ");
    }
}