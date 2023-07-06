using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/PC Stats SO", fileName = "New PC Stats SO")]
public class PCStatsSO : ScriptableObject
{
    public int combat = 1;
}
/*
    STATS(Still need to work these out)
        Scavenging - Affects quality of loot and loot time.
        Combat - Affects attack/resistance to injury/pain threshold/base defense ability and whatever combat stuff.
            (Maybe split combat up into separate stats? I like the one simple stat though.)
        Survival - Affects skill with growing crops/gathering water/medical skill, defending/building base? Not sure.
        Stealth - How quickly enemies hear you while scavenging.
        Medical - Ability to lower PC's injury level quickly/on the spot.
        Science/Technology/Engineering - Ability to research/build more advanced building/crafting items/equip. 
        Injury - How injured you are. Rest to recover.
    DERIVED STATS(Still need to work these out)
        Morale - Affects all stats of all PCs. Affected by many factors (entertainment buildings, food/water levels, recent deaths, etc.). 
            Affects all the stats of all PCs? Have it as a collective stat or separate for each PC? 
        Pain - Pain level.Affect stats negatively.
            Affected by: Injury level and certain items (painkillers, etc.).
        Pain threshold - How fast your pain increases with your injury level.
            Affected by: Combat, Survival?
        Attack Power - How effective your attacks are. Higher chance to hit, stronger melee hits, etc. = all add up to more DPS.
*/