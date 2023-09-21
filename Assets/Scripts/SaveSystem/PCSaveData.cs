using System.Collections.Generic;

/// <summary>
/// int PCID, int Injury, List{(StatType, int)} Stats, List{int} EquipmentIDs. <br/>
/// TODO - Using public fields instead of properties to get them to serialize. Is it necessary? 
/// </summary>
[System.Serializable]
public class PCSaveData
{
    /// <summary>
    /// Used as ID, and covers all the basic shared data. Load the rest from the other fields here. 
    /// Use SOPC database instead?
    /// </summary>
    public int PCID; /*{ get; set; }*/

    public int Injury; /*{ get; set; }*/

//    public List<(StatType, int)> Stats { get; set; } = new();
    public int Attack; /*{ get; set; }*/
    public int Defense; /*{ get; set; }*/
    public int Engineering; /*{ get; set; }*/
    public int Farming; /*{ get; set; }*/
    public int Medical; /*{ get; set; }*/
    public int Scavenging; /*{ get; set; }*/

    public List<int> EquipmentIDs; /*{ get; set; }*/

    public PCSaveData(
        int pcID,
        int injury,
        /*List<(StatType, int)> stats, */
        int attack, 
        int defense,
        int engineering,
        int farming,
        int medical,
        int scavenging,
        List<int> equipmentIDs)
    {
        PCID = pcID;

        Injury = injury;

//        Stats = stats;
        Attack = attack;
        Defense = defense;
        Engineering = engineering;
        Farming = farming;
        Medical = medical;
        Scavenging = scavenging;

        EquipmentIDs = equipmentIDs;
    }
}