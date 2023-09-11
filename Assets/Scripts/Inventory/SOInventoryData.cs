using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/SOInventoryData", fileName = "Inventory Data SO")]
public class SOInventoryData : SaveableSO
{
    /// <summary>
    /// TODO - Just put this on PCItemUseManager for now? Pass it down from GameManager's SOGameData. 
    /// </summary>
    [SerializeField]
    private SOCraftableItems _craftableItemsSO;

    [SerializeField]
    private SOInventory _usableInventorySO;
    [SerializeField]
    private SOInventory _equipmentInventorySO;
    [SerializeField]
    private SOInventory _craftingInventorySO;
    [SerializeField]
    private SOInventory _toolInventorySO;

    public SOCraftableItems CraftableItemsSO { get { return _craftableItemsSO; } }
    public SOInventory UsableItemsInventorySO { get { return _usableInventorySO; } }
    public SOInventory EquipmentInventorySO { get { return _equipmentInventorySO; } }
    public SOInventory CraftingInventorySO { get { return _craftingInventorySO; } }
    public SOInventory ToolInventorySO { get { return _toolInventorySO; } }
    public List<SOItem> PossibleCraftingRecipes { get; set; }
    public List<SOBuilding> PossibleBuildingRecipes { get; set; }

    public override void SaveData(GameData gameData)
    {
        UsableItemsInventorySO.SaveData(gameData);
        EquipmentInventorySO.SaveData(gameData);
        CraftingInventorySO.SaveData(gameData);
        ToolInventorySO.SaveData(gameData);
    }

    public override void LoadData(GameData gameData)
    {
        UsableItemsInventorySO.LoadData(gameData);
        EquipmentInventorySO.LoadData(gameData);
        CraftingInventorySO.LoadData(gameData);
        ToolInventorySO.LoadData(gameData);
    }
}