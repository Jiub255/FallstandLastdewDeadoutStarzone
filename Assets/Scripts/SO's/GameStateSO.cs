using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Game State", menuName = "Game State")]
public class GameStateSO : ScriptableObject
{
    public List<GameStateSO> transferrableToStates = new List<GameStateSO>();
    //public string stateActionMapName;

    public List<string> StateActionMaps = new List<string>();
}