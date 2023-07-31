using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/SOLoot", fileName = "New Loot SO")]
public class SOLoot : ScriptableObject
{
	[SerializeField]
	private List<ItemAmount> _loot;

	public List<ItemAmount> Loot { get { return _loot; } }
}