using System.Collections.Generic;
using UnityEngine;

// Fill this list before changing scenes with the PCs that will be in the next scene.
// After changing scenes, instantiate all the PCs from the list. 
[CreateAssetMenu(fileName = "New Available PCs SO", menuName = "Scriptable Object/Player Characters/Available PCs SO")]
public class AvailablePCsSO : ScriptableObject
{
	public List<PCItemSO> PCItemSOs = new List<PCItemSO>();
}