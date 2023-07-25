using System.Collections.Generic;
using UnityEngine;

// Put on each loot container
public class LootContainer : MonoBehaviour
{
    // TODO - Make LootSO and keep loot lists there instead, then you can just apply those to the prefab to change loot. 
    public List<ItemAmount> LootItemAmounts;
    public Transform LootPositionTransform; 

    public bool IsBeingLooted { get; set; } = false;
    public bool Looted { get; set; } = false;
}