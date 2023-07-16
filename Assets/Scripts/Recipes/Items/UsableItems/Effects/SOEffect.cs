using UnityEngine;

// Make other Effect classes inherit this and send out events. Then whichever script(s) that need to subscribe can. 
public abstract class SOEffect : ScriptableObject
{
	// Pass this (or at least the PCInstance from it) in the events the child classes send to apply effects, 
	// so the selected PC knows to recieve the effects and not all of them. 
	[SerializeField]
	protected SOListSOPC _currentTeamSO;

	public abstract void ApplyEffect(SOUsableItem item);
}