using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    // To store time/date of save, for finding most recent save for continue button
    public long LastUpdated { get; set; }


}