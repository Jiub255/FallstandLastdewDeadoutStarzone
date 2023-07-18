using UnityEngine;

public class UpdateManager : MonoBehaviour
{
	private ManagedUpdateBehaviour[] managedUpdateBehaviours;

	private void Start()
	{
		managedUpdateBehaviours = GetComponents<ManagedUpdateBehaviour>();
	}

	private void Update()
	{
		foreach (ManagedUpdateBehaviour managedUpdateBehaviour in managedUpdateBehaviours)
        {
			managedUpdateBehaviour.UpdateMe();
        }
	}
}