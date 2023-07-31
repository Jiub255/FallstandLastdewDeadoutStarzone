using UnityEngine;

[CreateAssetMenu(menuName = "Items/Crafting/SOCraftingItem", fileName = "New Crafting Item SO")]
public class SOCraftingItem : SOItem
{  
    // What to do when clicking crafting item in inventory?
    // Show possible recipes in pop up UI using this ingredient.
    public override void OnClickInventory()
    {
        Debug.Log($"Clicked on crafting item {name}");
    }
}