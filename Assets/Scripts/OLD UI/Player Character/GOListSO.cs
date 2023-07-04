using System.Collections.Generic;
using UnityEngine;

// TODO: Have an allPCsSO instance of this with all possible PCs, and have another
// instance with all "unlocked" PCs, and another for choosing/instancing your scavenging team. 
// Fill this list before changing scenes with the PCs that will be in the next scene.
// After changing scenes, instantiate all the PCs from the list. 
[CreateAssetMenu(fileName = "New GO List SO", menuName = "Scriptable Object/Player Characters/GO List SO")]
public class GOListSO : ScriptableObject
{
	public List<GameObject> GameObjects = new List<GameObject>();
}