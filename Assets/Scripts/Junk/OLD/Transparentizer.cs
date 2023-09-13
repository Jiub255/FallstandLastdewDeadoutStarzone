using System.Collections.Generic;
using UnityEngine;

public class Transparentizer : MonoBehaviour
{
    [SerializeField, Range(0f, 5f)]
    private float _size = 1f;
    [SerializeField, Range(0f, 1f)]
    private float _smoothness = 0.5f;
    [SerializeField, Range(0f, 1f)]
    private float _opacity = 1f;
	[SerializeField]
	private LayerMask _transparentableLayerMask;

    // TODO - Hopefully find a better fix for the PC box/sphere/ray cast eventually. 
    [SerializeField]
    private float _boxcastHalfWidth = 50f;
    [SerializeField]
    private float _boxcastHalfHeight = 50f;

    private float Size { get { return _size; } }
    private float Smoothness { get { return _smoothness; } }
    private float Opacity { get { return _opacity; } }
    private LayerMask TransparentableLayerMask { get { return _transparentableLayerMask; } }
    /// <summary>
    /// TODO - Hopefully find a better fix for the PC box/sphere/ray cast eventually. 
    /// </summary>
    private float BoxcastHalfWidth { get { return _boxcastHalfWidth; } }
    /// <summary>
    /// TODO - Hopefully find a better fix for the PC box/sphere/ray cast eventually. 
    /// </summary>
    private float BoxcastHalfHeight { get { return _boxcastHalfHeight; } }
    private Transform Transform { get; set; }
	private Transform CurrentPCTransform { get; set; }
	private Camera Camera { get; set; }
    private List<Material> FadedMaterials { get; } = new();
    // Just using a big number for now, figure out better amount later. 
    private Collider[] Colliders { get; } = new Collider[256];
    private List<Material> InTheWayMaterials { get; } = new();
    private int PCPositionID { get; } = Shader.PropertyToID("_PCPosition");
    private int SizeID { get; } = Shader.PropertyToID("_Size");
    private int SmoothnessID { get; } = Shader.PropertyToID("_Smoothness");
    private int OpacityID { get; } = Shader.PropertyToID("_Opacity");

    private void Start()
    {
/*        _mousePositionAction = S.I.IM.PC.Camera.MousePosition;
        _eventSystem = EventSystem.current;*/
        Transform = transform;
        Camera = Camera.main;
    }

    private void OnEnable()
    {
        PCSelector.OnSelectedNewPC += SelectPC;
    }

    private void OnDisable()
    {
        PCSelector.OnSelectedNewPC -= SelectPC;
    }

    private void SelectPC(SOPCData pcDataSO)
    {
        CurrentPCTransform = pcDataSO ? pcDataSO.PCInstance.transform : CurrentPCTransform;
    }

    // TODO - How to handle mouse position? Add to the shader graph? 
    private void FixedUpdate()
    {
        if (CurrentPCTransform != null)
        {
            // Get array of new in the way materials.

            // Halfway point between camera and PC for the box center. 
            Vector3 center = (Transform.position + CurrentPCTransform.position) / 2f;

            float boxHalfDepth = (CurrentPCTransform.position - Transform.position).magnitude / 2f;

            Vector3 halfExtents = new Vector3(
                BoxcastHalfWidth,
                BoxcastHalfHeight,
                boxHalfDepth);

            Quaternion orientation = Quaternion.LookRotation(CurrentPCTransform.position - Transform.position, Vector3.up);

            // Overlap box is cutting off parts of corner walls to the side of PC, somehow use a different shape/cast?
            // Ideally some sort of OverlapTetrahedron from camera bounds to PC would work. How to do something like this? 
            // Or some sort of OverlapCone actually? 
            // Could just do a "bundle" of raycasts to make a cone approximation. Walls are big so if the points are close enough it might work. 

            // Or, maybe do a spherecast from camera to PC, or right before PC so the tip of the sphere just touches. 
            // Then all colliders in that "tunnel" get faded. 
            // Make the circle cutout have a fixed world size or a fixed screen size, like it does now?
            // How would multiple walls work with fixed world size? It might look weird. 
            // Might stick with fixed screen size for now. Just make sure it's big enough to see the PC and at least some nearby area. 
            // But it can look too big when zoomed out too much. Maybe tie the circle size to the zoom level somehow to help, they're both camera scripts. 
            int maxIndex = Physics.OverlapBoxNonAlloc(
                center,
                halfExtents,
                Colliders,
                orientation,
                TransparentableLayerMask)
                // Subtract 1 to get max index from count. 
                - 1;

//            Debug.Log($"# of hits: {maxIndex + 1}, half extents: {halfExtents}, orientation: {orientation}");

            // Set PC screen position. 
            Vector3 pcPosition = Camera.WorldToViewportPoint(CurrentPCTransform.position);

            // Get all in the way materials. 
            InTheWayMaterials.Clear();
            // Only go up to maxIndex so you don't use empty entries in colliders array. 
            for (int i = 0; i < maxIndex; i++)
            {
                Renderer[] renderers = Colliders[i].transform.GetComponentsInChildren<Renderer>();
                foreach (Renderer renderer in renderers)
                {
                    InTheWayMaterials.AddRange(renderer.materials);
                }
            }

            // For each one, set its properties, and add it to the list. 
            foreach (Material material in InTheWayMaterials)
            {
                material.SetVector(PCPositionID, pcPosition);
                // TODO - Tie size to zoom level. Have to change CameraZoom as well to work with this. 
                // Make the cutout bigger the closer you get, play with it to find good values. 
                material.SetFloat(SizeID, Size);
                material.SetFloat(SmoothnessID, Smoothness);
                material.SetFloat(OpacityID, Opacity);

                if (!FadedMaterials.Contains(material))
                {
                    FadedMaterials.Add(material);
                }
            }

            // Get all items on faded materials that aren't on the in the way materials list (so they just stopped being in the way), 
            // and reset their properties (just set size to 0f, the others don't matter). 
            // Also remove material from _fadedMaterials. 
            for (int i = FadedMaterials.Count - 1; i >= 0; i--)
            {
                if (!InTheWayMaterials.Contains(FadedMaterials[i]))
                {
                    FadedMaterials[i].SetFloat(SizeID, 0f);

                    FadedMaterials.RemoveAt(i);
                }
            }
        }
        else
        {
            // Reset all faded materials. 
            if (FadedMaterials.Count > 0)
            {
                for (int i = FadedMaterials.Count - 1; i >= 0; i--)
                {
                    FadedMaterials[i].SetFloat(SizeID, 0f);

                    FadedMaterials.RemoveAt(i);
                }
            }
        }
    }

/*    private void OnDrawGizmos()
    {
        if (_currentPCTransform != null)
        {
            // Halfway point between camera and PC for the box center. 
            Vector3 center = (_transform.position + _currentPCTransform.position) / 2f;

            float boxHalfDepth = (_currentPCTransform.position - _transform.position).magnitude / 2f;

            Vector3 halfExtents = new Vector3(
                _boxcastHalfWidth,
                _boxcastHalfHeight,
                boxHalfDepth);

            Quaternion orientation = Quaternion.LookRotation(_currentPCTransform.position - _transform.position, Vector3.up);

            // create a matrix which translates an object by "position", rotates it by "rotation" and scales it by "halfExtends * 2"
            Gizmos.matrix = Matrix4x4.TRS(center, orientation, halfExtents * 2);
            // Then use it one a default cube which is not translated nor scaled
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        }
    }*/
}