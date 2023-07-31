using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/SOInventoryData", fileName = "Inventory Data SO")]
public class SOInventoryData : ScriptableObject
{
    [SerializeField]
    private SOInventory _usableInventorySO;
    [SerializeField]
    private SOInventory _equipmentInventorySO;
    [SerializeField]
    private SOInventory _craftingInventorySO;
    [SerializeField]
    private SOInventory _toolInventorySO;

    public SOInventory UsableInventorySO { get { return _usableInventorySO; } }
    public SOInventory EquipmentInventorySO { get { return _equipmentInventorySO; } }
    public SOInventory CraftingInventorySO { get { return _craftingInventorySO; } }
    public SOInventory ToolInventorySO { get { return _toolInventorySO; } }
    public List<SOItem> PossibleCraftingRecipes { get; set; }
    public List<SOBuilding> PossibleBuildingRecipes { get; set; }
}