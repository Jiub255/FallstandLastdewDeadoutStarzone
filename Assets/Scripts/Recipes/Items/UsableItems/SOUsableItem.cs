using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Usable Item SO", menuName = "Items/SOUsableItem")]
public class SOUsableItem : SOItem
{
    [SerializeField, Header("----------- Usable Item Data -----------")]
    private bool _reusable = false;
    [SerializeField]
    private List<SOEffect> _effects = new();

    public bool Reusable { get { return _reusable; } }
    public List<SOEffect> Effects { get { return _effects; } }

    public override void OnClickInventory()
    {
        Debug.Log($"Clicked on usable item {name}");

        foreach (SOEffect effect in Effects)
        {
            effect.ApplyEffect(this);
        }

        if (!Reusable)
        {
            RemoveFromInventory();
        }
    }
}