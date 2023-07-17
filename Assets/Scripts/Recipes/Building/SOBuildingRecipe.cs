using System;
using UnityEngine;

public class SOBuildingRecipe : SORecipe
{
    public static event Action<SOBuildingRecipe> OnSelectBuilding;

    public GameObject BuildingPrefab;

    // Called by button on Building Slot. 
    public override void OnClickRecipe()
    {
        // BuildingManager hears this.
        OnSelectBuilding?.Invoke(this);
    }
}