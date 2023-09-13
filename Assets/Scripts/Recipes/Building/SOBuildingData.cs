using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/SOBuildingData", fileName = "Building Data SO")]
public class SOBuildingData : ScriptableObject, IResettable
{
    [SerializeField]
    private SOBuildingRecipes _buildableBuildingsSO;
    [SerializeField]
    private SOBuildingDatabase _buildingDatabaseSO;
    [SerializeField]
    private GameObject _buildingParentPrefab;
    [SerializeField]
    private LayerMask _groundLayerMask;
    [SerializeField]
    private float _rotationSpeed = 50f;
    [SerializeField, Tooltip("Snap angle to the next [Snap Angle]. Works best with a divisor of 360.")]
    private int _snapAngle = 45;

    public SOBuildingRecipes BuildableBuildingsSO { get { return _buildableBuildingsSO; } }
    public SOBuildingDatabase BuildingDatabaseSO { get { return _buildingDatabaseSO; } }
    public GameObject BuildingParentPrefab { get { return _buildingParentPrefab; } }
    public LayerMask GroundLayerMask { get { return _groundLayerMask; } }
    public float RotationSpeed { get { return _rotationSpeed; } }
    public int SnapAngle { get { return _snapAngle; } }
    public SOBuilding CurrentBuildingRecipeSO { get; set; }
    public GameObject CurrentBuildingInstance { get; set; }
    /// <summary>
    /// TODO - Currently just using this list to check if required buildings are built to populate recipe lists. <br/>
    /// Do this using the List<BuildingLocation> instead. 
    /// </summary>
//    public List<SOBuilding> Buildings { get; set; }
    public List<BuildingLocation> BuildingLocations { get; set; }
    public SelectedBuildingIcon SelectedBuildingIcon { get; set; }
    public Quaternion Rotation { get; set; }

    /// <summary>
    /// Just for testing, remove before building game. 
    /// </summary>
    public void ResetOnExitPlayMode()
    {
        //Buildings.Clear();
    }

    public void SaveData(GameSaveData gameData)
    {
        // Is copying item by item necessary here? Could the whole list just be copied? 
        gameData.BuildingIDsAndLocations = new();
        foreach(BuildingLocation buildingLocation in BuildingLocations)
        {
            gameData.BuildingIDsAndLocations.Add(buildingLocation);
        }
    }

    public void LoadData(GameSaveData gameData)
    {
        BuildingLocations.Clear();
        foreach (BuildingLocation buildingLocation in gameData.BuildingIDsAndLocations)
        {
            BuildingLocations.Add(buildingLocation);
        }
    }
}