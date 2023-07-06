using UnityEngine;

public class SelectedPCIcon : MonoBehaviour
{
	private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void ActivateIcon(bool activate)
    {
//        Debug.Log($"ActivateIcon({activate}) called. ");
        _meshRenderer.enabled = activate;
    }

/*    public void DeactivateIcon()
    {
        Debug.Log("DeactivateIcon called. ");
        _meshRenderer.enabled = false;
    }*/
}