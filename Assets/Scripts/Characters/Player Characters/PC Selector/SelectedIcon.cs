using UnityEngine;

public class SelectedIcon : MonoBehaviour
{
	private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void ActivateIcon()
    {
        meshRenderer.enabled = true;
    }

    public void DeactivateIcon()
    {
        meshRenderer.enabled = false;
    }
}