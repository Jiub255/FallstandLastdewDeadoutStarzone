using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Building Item", menuName = "Items/Building Item")]
public class BuildingItemSO : ItemSO
{
    public static event Action<GameObject> OnSelectBuilding;

    public GameObject BuildingPrefab;

    // Building material cost, simple int for now.
        // Different amounts of different materials for cost later, maybe make a small cost class?
    public int Cost;

    public override void Use()
    {
        // Send signal to BuildingManager to change currentBuilding to this, but how?
        // Store prefab in SO? I think so.
        OnSelectBuilding?.Invoke(BuildingPrefab);

        base.Use();
    }
}

/*public class BuildCost
{
    public int woodCost;
    public int stoneCost;
    public int metalCost;
}*/