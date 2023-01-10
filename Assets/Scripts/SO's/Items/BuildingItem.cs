using UnityEngine;

[CreateAssetMenu(fileName = "Building Item", menuName = "Items/Building Item")]
public class BuildingItem : ItemSO
{
    // Building material cost, simple int for now.
        // Different amounts of different materials for cost later, maybe make a small cost class?
    public int cost;
    // 
}

/*public class BuildCost
{
    public int woodCost;
    public int stoneCost;
    public int metalCost;
}*/