using UnityEngine;

public class SelectedPCIcon : MonoBehaviour
{
	private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void ActivateIcon()
    {
        _meshRenderer.enabled = true;
    }

    public void DeactivateIcon()
    {
        _meshRenderer.enabled = false;
    }
}