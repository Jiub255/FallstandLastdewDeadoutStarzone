using UnityEngine;

[CreateAssetMenu(menuName = "Building/SOBuildingData", fileName = "Building Data SO")]
public class SOBuildingData : ScriptableObject
{
    [SerializeField]
    private GameObject _buildingPrefab;
    [SerializeField]
    private LayerMask _groundLayerMask;
    [SerializeField]
    private float _rotationSpeed = 50f;
    [SerializeField, Tooltip("Snap angle to the next [Snap Angle]. Works best with a divisor of 360.")]
    private int _snapAngle = 45;

    public GameObject BuildingPrefab { get { return _buildingPrefab; } }
    public LayerMask GroundLayerMask { get { return _groundLayerMask; } }
    public float RotationSpeed { get { return _rotationSpeed; } }
    public int SnapAngle { get { return _snapAngle; } }
    public SOBuilding CurrentBuildingRecipeSO { get; set; }
    public GameObject CurrentBuildingInstance { get; set; }
    public SelectedBuildingIcon SelectedBuildingIcon { get; set; }
    public Quaternion Rotation { get; set; }
}