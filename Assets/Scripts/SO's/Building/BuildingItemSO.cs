using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building Item SO", menuName = "Scriptable Object/Building/Building Item SO")]
public class BuildingItemSO : ItemSO
{
    public static event Action<GameObject> OnSelectBuilding;

    public GameObject BuildingPrefab;

    // Building material cost, simple int for now.
        // Different amounts of different materials for cost later, maybe make a small cost class?
    public int Cost { get; set; }

    public BuildingTypeSO buildingTypeSO;

    public override void Use()
    {
        // BuildingManager hears this.
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