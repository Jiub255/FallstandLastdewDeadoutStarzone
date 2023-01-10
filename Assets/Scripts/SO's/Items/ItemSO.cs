using UnityEngine;

// Have inventory items, building items, crafting items, etc all inherit from this
//[CreateAssetMenu(fileName = "Item", menuName = "Items/Item")]
public class ItemSO : ScriptableObject
{
    public Sprite itemIcon;
    [TextArea(3,20)]
    public string description;

    public virtual void Use()
    {
        Debug.Log("Using " + name);
    }
}