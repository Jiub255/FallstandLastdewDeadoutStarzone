using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildInventorySO", menuName = "Items/BuildInventorySO")]
public class BuildInventorySO : ScriptableObject
{
	public List<BuildingItem> buildItems = new List<BuildingItem>();
}