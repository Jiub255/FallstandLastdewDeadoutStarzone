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
    [SerializeField]
    private float _sceneTransitionFadeTime = 0.5f;

    public SOInventoryData InventoryDataSO { get { return _inventoryDataSO; } }
    public SOTeamData TeamDataSO { get { return _teamDataSO; } }
    public SOBuildingData BuildingDataSO { get { return _buildingDataSO; } }
    public GameObject UIPrefab { get { return _uiPrefab; } }
    public float SceneTransitionFadeTime { get { return _sceneTransitionFadeTime;} }

    // TODO - Keep GameState on here too? 
}