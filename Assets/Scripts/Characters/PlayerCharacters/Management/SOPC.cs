using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player Characters/SOPC", fileName = "New PC SO")]
public class SOPC : ScriptableObject
{
    // Or do Transform for PC instances? 
    public static event Action<GameObject> OnSelectPC;

    // These variables don't change during runtime. 
    public Sprite Icon;
    public Sprite CharacterImage;
    public GameObject PCPrefab;

    // These variables do change during runtime. 
    public GameObject PCInstance;
    public List<SOEquipmentItem> EquipmentItems;
    public int Injury;
    public List<Stat> Stats;

    // Called by clicking PC icon button. 
    public void Use()
    {
        // PCSelector hears this. Selects this PC for now. Center camera on double click later. 
        OnSelectPC?.Invoke(PCInstance);
    }
}