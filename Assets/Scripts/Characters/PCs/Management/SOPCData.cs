using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player Characters/SOPCData", fileName = "New PC Data SO")]
public class SOPCData : ScriptableObject
{
    // Or do Transform for PC instances? 
    public static event Action<GameObject> OnSelectPC;

    // SHARED
    public SOPCMovementState PCMovementStateSO; 

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
    public List<SOEquipmentItem> Equipment;
    // INDIVIDUAL
    public int Injury;
    // INDIVIDUAL - keep starting stats on shared, with randomization? 
    public Stats Stats;

    public int Attack
    {
        get
        { 
            return Stats[StatType.Attack].ModdedValue; 
        } 
    }
    public int Defense { get { return Stats[StatType.Defense].ModdedValue; } }

    // Called by clicking PC icon button. 
    public void Use()
    {
        // PCSelector hears this. Selects this PC for now. Center camera on double click later. 
        OnSelectPC?.Invoke(PCInstance);
    }
}