using UnityEngine;

/// <summary>
/// TODO - Add movement speed property, and apply it on instantiation. Same with other variables that are on the player game object, hidden in whatever monobehaviour.
/// Only a few to choose from luckily. Mostly the A* stuff. 
/// </summary>
[CreateAssetMenu(menuName = "States/PC/SOPCMovementState", fileName = "New PC Movement State SO")]
public class SOPCMovementState : ScriptableObject
{
	[SerializeField]
	private float _sightDistance;
/*	[SerializeField]
	private float _stoppingDistance;*/

	public float SightDistance { get { return _sightDistance; } }
//	public float StoppingDistanceSquared { get { return _stoppingDistance * _stoppingDistance; } }
}