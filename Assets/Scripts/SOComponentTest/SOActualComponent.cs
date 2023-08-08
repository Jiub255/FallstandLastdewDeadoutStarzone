using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SOActualComponent : SOComponent
{
    public override void MBAwake()
    {
        Debug.Log($"SOCO MBAwake, instance count: {GOInstances.Count}");
    }
}