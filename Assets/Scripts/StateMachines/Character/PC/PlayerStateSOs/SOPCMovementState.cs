using UnityEngine;

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