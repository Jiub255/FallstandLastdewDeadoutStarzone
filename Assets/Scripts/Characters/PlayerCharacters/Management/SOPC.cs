using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player Characters/SOPC", fileName = "New PC SO")]
public class SOPC : ScriptableObject
{
    // Or do Transform for PC instances? 
    public static event Action<GameObject> OnSelectPC;

    public readonly Sprite Icon;
    public readonly Sprite CharacterImage;
    public readonly GameObject PCPrefab;

    public GameObject PCInstance;

    public List<SOEquipmentItem> EquipmentItems;

    public int TruePain;
    public int Injury;

    public List<Stat> Stats;


    // Called by clicking PC icon button. 
    public void Use()
    {
        // PCSelector hears this. Selects this PC for now. Center camera on double click later. 
        OnSelectPC?.Invoke(PCInstance);
    }
}