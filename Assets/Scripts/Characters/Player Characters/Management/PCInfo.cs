using System;
using UnityEngine;

public class PCInfo : MonoBehaviour
{
    // Or do Transform for PC instances? 
    public static event Action<GameObject> OnSelectPC;

    public Sprite Icon;

    public void Use()
    {
        // PCSelector hears this. Selects this PC for now. Center camera on double click later. 
        OnSelectPC?.Invoke(transform.parent.gameObject);
    }
}