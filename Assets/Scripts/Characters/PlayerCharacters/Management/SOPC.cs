using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player Characters/SOPC", fileName = "New PC SO")]
public class SOPC : ScriptableObject
{
    // SHARED
    // Or do Transform for PC instances? 
    public static event Action<GameObject> OnSelectPC;

    // These variables don't change during runtime. 
    // INDIVIDUAL
    public Sprite Icon;
    // INDIVIDUAL
    public Sprite CharacterImage;
    // INDIVIDUAL
    public GameObject PCPrefab;

    // These variables do change during runtime. 
    // INDIVIDUAL
    public GameObject PCInstance;
    // INDIVIDUAL
    public List<SOEquipmentItem> EquipmentItems;
    // INDIVIDUAL
    public int Injury;
    // INDIVIDUAL - MAYBE KEEP STARTING STATS ON SHARED? 
    public List<Stat> Stats;

    // Called by clicking PC icon button. 
    // SHARED
    public void Use()
    {
        // PCSelector hears this. Selects this PC for now. Center camera on double click later. 
        OnSelectPC?.Invoke(PCInstance);
    }
}