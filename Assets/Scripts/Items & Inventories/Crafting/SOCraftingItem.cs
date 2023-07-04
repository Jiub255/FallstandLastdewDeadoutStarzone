using UnityEngine;

[CreateAssetMenu(fileName = "New Crafting Item SO", menuName = "Items/SOCraftingItem")]
public class SOCraftingItem : SOItem
{  
    // What to do when clicking crafting item in inventory?
    // Show possible recipes in pop up UI using this ingredient.
    public override void OnClickFromInventory()
    {
        Debug.Log($"Clicked on crafting item {name}");
    }

    // When clicking on item in crafting menu, add item to items to be used in recipe. 
    public void OnClickFromCrafting()
    {

    }
}