using UnityEngine;

// Have inventory items, building items, crafting items, etc all inherit from this
public abstract class ItemSO : ScriptableObject
{
    public Sprite ItemIcon;
    [TextArea(3, 20)]
    public string Description;

    public virtual void Use()
    {
        //Debug.Log("Using " + name);
    }
}