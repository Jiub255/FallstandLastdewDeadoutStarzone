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
        // Want to bring up a popup menu that lets you choose which PC to apply the item effect to. 
        // OR, have it like the equipment screen where you have a certain PC 'selected', and when you use the item
        // it gets applied to them. You can click on arrows to change selected PC, same as in equipment menu. 
        ItemEffect?.Invoke();

        Debug.Log("Used " + name);
    }
}