using System;
using UnityEngine;

public enum BuildingType
{
    Medical,
    Defensive,
    Crafting,
    QOL,
    Farming // Or production? 
}

public class SOBuildingRecipe : SORecipe
{
    public static event Action<SOBuildingRecipe> OnSelectBuilding;

    private BuildingType _buildingType;
    private GameObject _buildingPrefab;

    public BuildingType BuildingType { get { return _buildingType; } }
    public GameObject BuildingPrefab { get { return _buildingPrefab; } }

    // Called by button on Building Slot. 
    public override void OnClickRecipe()
    {
        // BuildingManager hears this.
        OnSelectBuilding?.Invoke(this);
    }
}