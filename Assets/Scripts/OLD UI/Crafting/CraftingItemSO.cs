using UnityEngine;

[CreateAssetMenu(fileName = "New Crafting Item SO", menuName = "Scriptable Object/Crafting/Crafting Item SO")]
public class CraftingItemSO : ItemSO
{ 
    public override void Use()
    {
        // Popup menu showing what can be built using this item. Stuff you are able to build in the beginning of the list. 
        // Clicking on whatever item brings you to the crafting page for that item. 
    }
}