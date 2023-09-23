using System.Collections.Generic;

/// <summary>
/// Have to use public fields instead of properties so that they can be serialized into JSON. <br/>
/// Would [SerializeField] private and public properties work? Does ToJson work with serialize field private fields? 
/// </summary>
[System.Serializable]
public class GameSaveData
{
    // To store time/date of save, for finding most recent save for continue button
    public long LastUpdated;

    // Inventory
    public List<SerializableDouble<int, int>> ItemIDAmountTuples = new();

    // Buildings
    public List<BuildingLocation> BuildingIDsAndLocations = new();

    // PCs
    // How to keep each PC's equipment, injury level, and stats? 
    // Could make a specific PCSaveData serializable struct and use that? 
    // It could have: 
    // PC database ID (to populate SOTeamData), SOEquipmentItem database IDs, List<Stat>, int injury. 
    public List<PCSaveData> HomePCs = new();
    public List<int> ScavengingPCIDs = new();

    public GameSaveData()
    {
        LastUpdated = System.DateTime.Now.ToBinary();
        ItemIDAmountTuples = new List<SerializableDouble<int, int>>();
        BuildingIDsAndLocations = new List<BuildingLocation>();
        HomePCs = new List<PCSaveData>();
        ScavengingPCIDs = new List<int>();

        // TODO - Just for testing. Do it betterly eventually. 
        // Set up stats. 
/*        List<(StatType, int)> startingStats = new();
        startingStats.Add((StatType.Attack, 0));
        startingStats.Add((StatType.Defense, 0));
        startingStats.Add((StatType.Engineering, 0));
        startingStats.Add((StatType.Farming, 0));
        startingStats.Add((StatType.Medical, 0));
        startingStats.Add((StatType.Scavenging, 0));*/

        // TODO - Put default/starting stats on shared PC Data SO or something? 
        // Keep PC Template data separate from saveable data and other data that changes during game play. 

        // Add starting PC to HomePCs list.
        PCSaveData pcSaveData = new(
            0,
            0, 
//            startingStats,
            1,
            1,
            1,
            1,
            1,
            1,
            new List<int>());

        HomePCs.Add(pcSaveData);
    }
}

// TODO - Try using SerializableDouble instead first. 
/// <summary>
/// Using this instead of a tuple or dictionary so it can be serialized. 
/// </summary>
/*[System.Serializable]
public class ItemIDAmount
{
    public int ItemID;
    public int Amount;
}*/