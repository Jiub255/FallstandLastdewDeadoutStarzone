using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player Characters/SOListSOPC", fileName = "New SOPC List SO")]
public class SOListSOPC : ScriptableObject
{
	public List<SOPC> SOPCs = new();
}