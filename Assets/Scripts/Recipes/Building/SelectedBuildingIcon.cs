using UnityEngine;

// TODO - Handle changing highlight from green to red here too. Instead of that slop in BuildingManager. 
public class SelectedBuildingIcon : MonoBehaviour
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