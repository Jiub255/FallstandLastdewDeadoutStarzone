using UnityEngine;

public class ManagedUpdateBehaviour : MonoBehaviour
{
	public virtual void UpdateMe()
    {
        Debug.Log("UpdateMe called");
    }
}