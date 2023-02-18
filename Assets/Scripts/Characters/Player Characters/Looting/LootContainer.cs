using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Put on each loot container
public class LootContainer : MonoBehaviour
{
    public List<ItemAmount> LootItemAmounts;

    public bool IsBeingLooted { get; set; } = false;
    public bool Looted { get; set; } = false;

    public Transform LootPosition; 
}