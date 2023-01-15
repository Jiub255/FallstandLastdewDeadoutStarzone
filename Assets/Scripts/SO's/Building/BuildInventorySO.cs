using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Build Inventory SO", menuName = "Scriptable Object/Building/Build Inventory SO")]
public class BuildInventorySO : ScriptableObject
{
	public List<BuildingSO> BuildItems = new List<BuildingSO>();
}