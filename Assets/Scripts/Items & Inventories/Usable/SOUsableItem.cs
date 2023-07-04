using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Usable Item SO", menuName = "Items/SOUsableItem")]
public class SOUsableItem : SOItem
{
    public bool IsReusable = false;

    public List<SOEffect> Effects = new();

    public override void OnClickFromInventory()
    {
        Debug.Log($"Clicked on usable item {name}");

        foreach (SOEffect effect in Effects)
        {
            effect.ApplyEffect(this);
        }

        if (!IsReusable)
        {
            RemoveFromInventory();
        }
    }
}