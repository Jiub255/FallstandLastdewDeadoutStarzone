using UnityEngine;

[CreateAssetMenu(menuName = "Data/SOGameData", fileName = "Game Data SO")]
public class SOGameData : SaveableSO
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

    public override void LoadData(GameData gameData)
    {
        InventoryDataSO.LoadData(gameData);
        TeamDataSO.LoadData(gameData);
        BuildingDataSO.LoadData(gameData);
    }

    /// <summary>
    /// Do this and just chain down until you have all the data needed. Similar for loading. <br/>
    /// This way only need to call SaveData and LoadData once here, using DataPersistenceManager or whatever, and the methods will chain downwards. <br/>
    /// Easier and cleaner than trying to gather all the SaveableSOs and call the methods on each individually. This way takes advantage of the data's structure. 
    /// </summary>
    /// <param name="gameData"></param>
    public override void SaveData(GameData gameData)
    {
        InventoryDataSO.SaveData(gameData);
        TeamDataSO.SaveData(gameData);
        BuildingDataSO.SaveData(gameData);
    }

    // TODO - Keep GameState on here too? 
}