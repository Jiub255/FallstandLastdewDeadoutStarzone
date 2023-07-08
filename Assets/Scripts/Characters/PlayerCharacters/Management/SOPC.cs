using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Player Characters/SOPC", fileName = "New PC SO")]
public class SOPC : ScriptableObject
{
    // Or do Transform for PC instances? 
    public static event Action<GameObject> OnSelectPC;

    public Sprite Icon;
    public GameObject PCPrefab;
    [HideInInspector]
    public GameObject PCInstance;/* { get; set; }*/

    // Called by clicking PC icon button. 
    public void Use()
    {
        // PCSelector hears this. Selects this PC for now. Center camera on double click later. 
        OnSelectPC?.Invoke(PCInstance);
    }
}