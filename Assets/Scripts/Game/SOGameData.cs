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
    [SerializeField]
    private GameObject _uiPrefab;

    public SOInventoryData InventoryDataSO { get { return _inventoryDataSO; } }
    public SOTeamData TeamDataSO { get { return _teamDataSO; } }
    public SOBuildingData BuildingDataSO { get { return _buildingDataSO; } }
    public GameObject UIPrefab { get { return _uiPrefab; } }
}