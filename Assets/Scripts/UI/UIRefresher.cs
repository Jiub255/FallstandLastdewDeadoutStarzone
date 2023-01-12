using UnityEngine;

// Could probably use this for the BuildUI too. Just make a new build inventorySlot prefab with whatever button function on it.
// I suppose crafting too, and equipment. Just set up the slot/button prefabs and the scroll views correctly.
// At least can do an abstract class and inherit for each type of UI
public abstract class UIRefresher : MonoBehaviour
{
    protected GameObject SlotPrefab;

    protected Transform Content;

    public virtual void PopulateInventory()
    {
        ClearInventory();
    }

    private void ClearInventory()
    {
        foreach (Transform child in Content)
        {
            Destroy(child.gameObject);
        }
    }
}