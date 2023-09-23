using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/SOInventoryData", fileName = "Inventory Data SO")]
public class SOInventoryData : ScriptableObject
{
    /// <summary>
    /// TODO - Just put this on PCItemUseManager for now? Pass it down from GameManager's SOGameData. 
    /// </summary>
    [SerializeField]
    private SOCraftableItems _craftableItemsSO;
    [SerializeField] 
    private SOItemDatabase _itemDatabaseSO;

    [SerializeField]
    private SOInventory _usableInventorySO;
    [SerializeField]
    private SOInventory _equipmentInventorySO;
    [SerializeField]
    private SOInventory _craftingInventorySO;
    [SerializeField]
    private SOInventory _toolInventorySO;

    public SOCraftableItems CraftableItemsSO { get { return _craftableItemsSO; } }
    public SOItemDatabase ItemDatabaseSO { get { return _itemDatabaseSO; } }
    public SOInventory UsableItemsInventorySO { get { return _usableInventorySO; } }
    public SOInventory EquipmentInventorySO { get { return _equipmentInventorySO; } }
    public SOInventory CraftingInventorySO { get { return _craftingInventorySO; } }
    public SOInventory ToolInventorySO { get { return _toolInventorySO; } }
    public List<SOItem> PossibleCraftingRecipes { get; set; }
    public List<SOBuilding> PossibleBuildingRecipes { get; set; }

    /// <summary>
    /// IndexOf not working here, returning -1. <br/>
    /// Instead of inventory and equipment holding references to the actual items, have them hold the database ID only,
    /// then do everything through the database. 
    /// </summary>
    public void SaveData(GameSaveData gameData)
    {
        // Go through all the inventories and just save all the ItemAmounts onto one big list. 
        // When loading, add all the ItemAmounts through InventoryManager's AddItems method and they will get sorted back into 
        // the correct inventories based on their subtype. 
        gameData.ItemIDAmountTuples.Clear();

        foreach (ItemAmount itemAmount in UsableItemsInventorySO.ItemAmounts)
        {
            gameData.ItemIDAmountTuples.Add(new SerializableDouble<int, int>
                (ItemDatabaseSO.Items.IndexOf(itemAmount.ItemSO),
                itemAmount.Amount));
        }
        foreach (ItemAmount itemAmount in EquipmentInventorySO.ItemAmounts)
        {
            gameData.ItemIDAmountTuples.Add(new SerializableDouble<int, int>
                (ItemDatabaseSO.Items.IndexOf(itemAmount.ItemSO),
                itemAmount.Amount));
        }
        foreach (ItemAmount itemAmount in CraftingInventorySO.ItemAmounts)
        {
            gameData.ItemIDAmountTuples.Add(new SerializableDouble<int, int>
                (ItemDatabaseSO.Items.IndexOf(itemAmount.ItemSO),
                itemAmount.Amount));
        }
        foreach (ItemAmount itemAmount in ToolInventorySO.ItemAmounts)
        {
            gameData.ItemIDAmountTuples.Add(new SerializableDouble<int, int>
                (ItemDatabaseSO.Items.IndexOf(itemAmount.ItemSO),
                itemAmount.Amount));
        }
    }
}