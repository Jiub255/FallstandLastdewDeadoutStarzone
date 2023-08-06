using System;
using UnityEngine;

/// <summary>
/// TODO - Use this enum or have subclasses? What exactly will the different buildings do? 
/// </summary>
public enum BuildingType
{
    Medical,
    Defensive,
    Crafting,
    QOL,
    Farming, // Or production? 
    Storage
}

[CreateAssetMenu(menuName = "Building/SOBuilding", fileName = "New Building SO")]
public class SOBuilding : SORecipe
{
    /// <summary>
    /// BuildingManager listens, sets this as current building SO. 
    /// </summary>
    public static event Action<SOBuilding> OnSelectBuilding;

	[SerializeField, Header("------------ Building Data -------------")]
    private BuildingType _buildingType;
    [SerializeField]
    private GameObject _buildingPrefab;

    public BuildingType BuildingType { get { return _buildingType; } }
    public GameObject BuildingPrefab { get { return _buildingPrefab; } }

    /// <summary>
    /// Called by button on Building Slot. 
    /// </summary>
    public override void OnClickRecipe()
    {
        OnSelectBuilding?.Invoke(this);
    }
}