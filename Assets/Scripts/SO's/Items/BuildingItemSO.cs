using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Building Item", menuName = "Items/Building Item")]
public class BuildingItemSO : ItemSO
{
    public GameObject buildingPrefab;

    // Building material cost, simple int for now.
        // Different amounts of different materials for cost later, maybe make a small cost class?
    public int cost;

    public static event Action<GameObject> onSelectBuilding;

    public override void Use()
    {
        // Send signal to BuildingManager to change currentBuilding to this, but how?
        // Store prefab in SO? I think so.
        onSelectBuilding?.Invoke(buildingPrefab);

        base.Use();
    }
}

/*public class BuildCost
{
    public int woodCost;
    public int stoneCost;
    public int metalCost;
}*/