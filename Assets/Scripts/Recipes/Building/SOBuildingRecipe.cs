using System;
using UnityEngine;

public class SOBuildingRecipe : SORecipe
{
    public static event Action<GameObject> OnSelectBuilding;

    public GameObject BuildingPrefab;

    // Called by button on Building Slot. 
    public override void OnClickRecipe()
    {
        // BuildingManager hears this.
        OnSelectBuilding?.Invoke(BuildingPrefab);
    }
}