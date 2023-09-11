using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Have all SOs with data that needs to be saved inherit this instead of ScriptableObject. 
/// </summary>
public abstract class SaveableSO : ScriptableObject
{
	public abstract void SaveData(GameData gameData);
	public abstract void LoadData(GameData gameData);
}