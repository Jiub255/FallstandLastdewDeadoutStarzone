using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Usable Item SO", menuName = "Scriptable Object/Usable/Usable Item SO")]
public class UsableItemSO : ItemSO
{
    // Function/effect?
    public UnityEvent ItemEffect;

    // Craftable stuff. 
    public bool Craftable = false;
    [NaughtyAttributes.ShowIf("Craftable")]
    public List<CraftingItemAmount> CraftingCost = new List<CraftingItemAmount>();

    // Gets called when you click on the item in inventory. 
    public override void Use()
    {
        ItemEffect?.Invoke();
    }
}