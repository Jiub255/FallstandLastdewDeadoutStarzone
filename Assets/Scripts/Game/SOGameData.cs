using UnityEngine;

[CreateAssetMenu(menuName = "Data/SOGameData", fileName = "Game Data SO")]
public class SOGameData : ScriptableObject
{
    [SerializeField]
    private SOInventoryData _inventoryDataSO;
    [SerializeField]
    private SOTeamData _teamDataSO;
    [SerializeField]
    private SOBuildingData _buildingDataSO;

    public SOInventoryData InventoryDataSO { get { return _inventoryDataSO; } }
    public SOTeamData TeamDataSO { get { return _teamDataSO; } }
    public SOBuildingData BuildingDataSO { get { return _buildingDataSO; } }
}