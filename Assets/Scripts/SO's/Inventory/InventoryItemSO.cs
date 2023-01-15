using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Inventory Item SO", menuName = "Scriptable Object/Inventory/Inventory Item SO")]
public class InventoryItemSO : ItemSO
{
    // Function/effect?
    // Might have multiple types of InventoryItem (child classes?): Building material, crafting material, usable item, equipment item.
    // Or just show usable item in inv, and have building/crafting/equipment materials shown in build/craft/equip menus.
    // That'd be cleaner, no need to have inventory cluttered with stuff you're not using at the moment. 
    public UnityEvent ItemEffect;

    public override void Use()
    {
        base.Use();

        ItemEffect?.Invoke();
    }
}