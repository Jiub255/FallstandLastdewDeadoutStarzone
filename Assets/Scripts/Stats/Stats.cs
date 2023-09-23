using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TODO - Hard code the specific stats in instead? Like attack, farming, whatever, instead of having this stat list? <br/>
/// Might be easier to save/load, and just keep track of in general. Do Stats.Attack instead of using this weird search index thing. <br/>
/// Plus the stats are already hard coded in the enum. Just hard code them here instead. 
/// </summary>
// Only serializable to show up in editor. 
[System.Serializable]
public class Stats : IEnumerable<Stat>
{
    [SerializeField]
    private Stat _attack = new(StatType.Attack, 1);
    [SerializeField]
    private Stat _defense = new(StatType.Defense, 1);
    [SerializeField]
    private Stat _engineering = new(StatType.Engineering, 1);
    [SerializeField]
    private Stat _farming = new(StatType.Farming, 1);
    [SerializeField]
    private Stat _medical = new(StatType.Medical, 1);
    [SerializeField]
    private Stat _scavenging = new(StatType.Scavenging, 1);
    
    public Stat Attack { get { return _attack; } set { _attack = value; } }
    public Stat Defense { get { return _defense; } set { _defense = value; } }
    public Stat Engineering { get { return _engineering; } set { _engineering = value; } }
    public Stat Farming { get { return _farming; } set { _farming = value; } }
    public Stat Medical { get { return _medical; } set { _medical = value; } }
    public Stat Scavenging { get { return _scavenging; } set { _scavenging = value; } }

    public IEnumerator<Stat> GetEnumerator()
    {
        yield return Attack;
        yield return Defense;
        yield return Engineering;
        yield return Farming;
        yield return Medical;
        yield return Scavenging;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

/*	[SerializeField]
	private List<Stat> _statList = new();
	public List<Stat> StatList { get { return _statList; } }*/

/*	public Stat this[StatType statType]
    {
        get
        {
            foreach (Stat stat in StatList)
            {
                if (stat.StatType == statType)
                {
                    return stat;
                }
            }

            Debug.LogWarning($"No stat of type {statType} found in Statlist.");
            return null;
        }
    }*/
}