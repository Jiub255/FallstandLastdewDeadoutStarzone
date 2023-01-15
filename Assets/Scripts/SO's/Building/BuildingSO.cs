using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building SO", menuName = "Scriptable Object/Building/Building SO")]
public class BuildingSO : ItemSO
{
    public static event Action<GameObject> OnSelectBuilding;

    public GameObject BuildingPrefab;

    // Building material cost, simple int for now.
        // Different amounts of different materials for cost later, maybe make a small cost class?
    public int Cost { get; set; }

    public override void Use()
    {
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