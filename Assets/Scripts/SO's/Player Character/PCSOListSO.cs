using System.Collections.Generic;
using UnityEngine;

// TODO: Rename this PCSOListSO or something, and have an allPCsSO instance with all possible PCs, and have another
// instance with all "unlocked" PCs, and another for choosing/instancing your scavenging team. 
// Fill this list before changing scenes with the PCs that will be in the next scene.
// After changing scenes, instantiate all the PCs from the list. 
[CreateAssetMenu(fileName = "New PCSO List SO", menuName = "Scriptable Object/Player Characters/PCSO List SO")]
public class PCSOListSO : ScriptableObject
{
	public List<PCItemSO> PCItemSOs = new List<PCItemSO>();
}