using System.Collections.Generic;

[System.Serializable]
public class GameSaveData
{
    // To store time/date of save, for finding most recent save for continue button
    public long LastUpdated { get; set; }

    // Inventory
    public List<(int, int)> ItemIDAmountTuples { get; } = new();

    // Buildings
    public List<BuildingLocation> BuildingIDsAndLocations { get; set; } = new();

    // PCs
    // How to keep each PC's equipment, injury level, and stats? 
    // Could make a specific PCSaveData serializable struct and use that? 
    // It could have: 
    // PC database ID (to populate SOTeamData), SOEquipmentItem database IDs, List<Stat>, int injury. 
    public List<PCSaveData> HomePCs { get; } = new();
    public List<int> ScavengingPCIDs { get; } = new();

    public GameSaveData()
    {
        LastUpdated = System.DateTime.Now.ToBinary();
        ItemIDAmountTuples = new List<(int, int)>();
        BuildingIDsAndLocations = new List<BuildingLocation>();
        HomePCs = new List<PCSaveData>();
        ScavengingPCIDs = new List<int>();
    }
}