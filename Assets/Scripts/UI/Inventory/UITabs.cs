using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls switching tabs (Usable, Crafting, and Equipment) in inventory. 
public class UITabs : MonoBehaviour
{
	[SerializeField]
	private GameObject _usableItemsPanel;
	[SerializeField]
	private GameObject _equipmentItemsPanel;
	[SerializeField]
	private GameObject _craftingItemsPanel;
}