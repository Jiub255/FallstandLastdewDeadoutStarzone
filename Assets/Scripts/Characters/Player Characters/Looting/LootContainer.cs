using System.Collections.Generic;
using UnityEngine;

// Put on each loot container
public class LootContainer : MonoBehaviour
{
    public List<OLDItemAmount> LootItemAmounts;

    public bool IsBeingLooted { get; set; } = false;
    public bool Looted { get; set; } = false;

    public Transform LootPositionTransform; 
}