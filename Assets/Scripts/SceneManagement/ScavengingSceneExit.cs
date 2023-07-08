using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScavengingSceneExit : MonoBehaviour
{
	[SerializeField]
	private SOListSOPC _scavengingTeamSO;

	// Have all PCs walk out of exit.

	// Once last PC leaves, load next scene (Scavenging scene results menu). 
	// Show loot gained, condition of team members, enemies killed, time taken, whatever other info. 
}