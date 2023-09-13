using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManagedUpdateBehaviour : ManagedUpdateBehaviour
{
    public override void UpdateMe()
    {
        base.UpdateMe();

        Debug.Log("UpdateMe override method called");
    }
}