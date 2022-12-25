using UnityEngine;

// Have inventory items, building items, crafting items, etc all inherit from this
//[CreateAssetMenu(fileName = "Item", menuName = "Items/Item")]
public class Item : ScriptableObject
{
    public Sprite itemIcon;
    public string description;
}