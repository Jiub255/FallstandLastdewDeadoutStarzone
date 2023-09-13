using System.Collections.Generic;

/// <summary>
/// int PCID, int Injury, List{(StatType, int)} Stats, List{int} EquipmentIDs. 
/// </summary>
[System.Serializable]
public class PCSaveData
{
    /// <summary>
    /// Used as ID, and covers all the basic shared data. Load the rest from the other fields here. 
    /// Use SOPC database instead? 
    /// </summary>
    public int PCID { get; set; }

    public int Injury { get; set; }
    public List<(StatType, int)> Stats { get; set; } = new();
    // TODO - Use database/IDs instead? 
    public List<int> EquipmentIDs { get; set; } = new();

    public PCSaveData(int pcID, int injury, List<(StatType, int)> stats, List<int> equipmentIDs)
    {
        PCID = pcID;
        Injury = injury;
        Stats = stats;
        EquipmentIDs = equipmentIDs;
    }
}