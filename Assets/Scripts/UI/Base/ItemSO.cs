using UnityEngine;

// Have inventory items, building items, crafting items, etc all inherit from this
public abstract class ItemSO : ScriptableObject
{
    public Sprite Icon;
    [TextArea(3, 20)]
    // TODO: Make the description show up in a pop up text box when hovering over button.
    public string Description;

    public /*virtual*/ abstract void Use();
}