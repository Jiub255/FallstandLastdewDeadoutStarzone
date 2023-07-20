using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player Characters/SOPC", fileName = "New PC SO")]
public class SOPC : ScriptableObject
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
    public List<SOEquipmentItem> EquipmentItems;
    // INDIVIDUAL
    public int Injury;
    // INDIVIDUAL - MAYBE KEEP STARTING STATS ON SHARED? 

    // TODO - What to do here? SerializableDictionary is getting to be more work than it's worth. 
    // Maybe just make it into a special stat list class, with the methods it needs. Including all the modifier stuff and the dictionary stuff. 
    public SerializableDictionary<StatType, Stat> StatsSerDict;
    public List<Stat> Stats;

    // Called by clicking PC icon button. 
    // SHARED
    public void Use()
    {
        // PCSelector hears this. Selects this PC for now. Center camera on double click later. 
        OnSelectPC?.Invoke(PCInstance);
    }


/*    public int Attack()
    {
        int attackStat;
        foreach (Stat stat in Stats)
        {
            if (stat.StatTypeSO.name == "Attack")
            {
                attackStat = stat.ModdedValue;
            }
        }
    }*/
}