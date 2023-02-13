using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Usable Item SO", menuName = "Scriptable Object/Usable/Usable Item SO")]
public class UsableItemSO : ItemSO
{
    // Function/effect?
    public UnityEvent ItemEffect;

    public override void Use()
    {
        //base.Use();

        ItemEffect?.Invoke();
    }
}