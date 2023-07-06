using UnityEngine;

// TODO - Handle changing highlight from green to red here too. Instead of that slop in BuildingManager. 
public class SelectedBuildingIcon : MonoBehaviour
{
    [SerializeField]
    private Material _greenMaterial;
    [SerializeField]
    private Material _redMaterial;

    [SerializeField]
    private float _borderThickness = 1f;
    [SerializeField]
    private float _height = 0.1f;

    private Transform _transform;
	private Renderer _renderer;
    private BoxCollider _boxCollider;

    private void Awake()
    {
        _transform = transform;
        _renderer = GetComponent<Renderer>();
        _boxCollider = transform.parent.GetComponentInChildren<BoxCollider>();

        SetIconSize();
    }

    private void SetIconSize()
    {
        // Set icon size based off collider, border thickness, and a y value for height. 
        Vector3 position = _boxCollider.transform.position;
        Vector3 size = _boxCollider.size;
        Debug.Log($"Size: {_boxCollider.size}");
        
        // Set position, including height. 
        _transform.position = new Vector3(position.x, 0f, position.z);

        // Set size with border thickness added (need to scale thickness by parent's scale). 
        _transform.localScale = new Vector3(
            size.x + (_borderThickness / _transform.parent.localScale.x),
            _height * 2f,
            size.z + (_borderThickness / _transform.parent.localScale.z));
    }

    public void ActivateIcon()
    {
        _renderer.enabled = true;
    }

    public void DeactivateIcon()
    {
        _renderer.enabled = false;
    }

    public void SetRedMaterial()
    {
        _renderer.material = _redMaterial;
    }

    public void SetGreenMaterial()
    {
        _renderer.material = _greenMaterial;
    }
}