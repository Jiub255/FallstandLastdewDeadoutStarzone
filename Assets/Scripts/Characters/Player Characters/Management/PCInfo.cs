using System;
using UnityEngine;

public class PCInfo : MonoBehaviour
{
    public static event Action<Transform> OnSelectPC;

    [SerializeField]
    private Sprite _icon;

    public void Use()
    {
        // PCSelector hears this. Selects this PC for now. Center camera on double click later. 
        OnSelectPC?.Invoke(transform.parent);
    }
}