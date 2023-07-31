using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/SOGameData", fileName = "Game Data SO")]
public class SOGameData : ScriptableObject
{
    [SerializeField]
    private SOInventoryData _inventoryDataSO;
    [SerializeField]
    private SOCurrentTeam _currentTeamSO;
    [SerializeField]
    private SOBuildingData _buildingDataSO;

    public SOInventoryData InventoryDataSO { get { return _inventoryDataSO; } }
    public SOCurrentTeam CurrentTeamSO { get { return _currentTeamSO; } }
    public SOBuildingData BuildingDataSO { get { return _buildingDataSO; } }
    /// <summary>
    /// Using dictionary instead of StatValue so you can change value and get by key. 
    /// </summary>
    /// <remarks>
    /// TODO - Put these in SOStatsData? Like how SOInventoryData works? 
    /// </remarks>
    public Dictionary<StatType, int> IndividualPCStatMaxes { get; }
}