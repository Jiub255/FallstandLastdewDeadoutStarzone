using UnityEngine;

public abstract class SOEffect : ScriptableObject
{
	// Make other Effect classes inherit this and send out events. Then whichever script(s) that need to subscribe can. 
	public abstract void ApplyEffect(SOUsableItem item);
}