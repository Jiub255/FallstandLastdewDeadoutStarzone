using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

// Put this on camera
public class Transparentizer : MonoBehaviour
{
    // Could make multiple transparentable layers if needed, then collect them all into this LayerMask. 
    [SerializeField]
    private LayerMask _transparentableLayerMask;

    // Currently selected PC
    private Transform _currentPCTransform;

    // A material should be on at most one of these next three lists/dicts at once
    // List of all materials that are currently fully faded out
    private List<Material> _fadedOut = new List<Material>();

    // Dictionary of all materials/coroutines that are currently fading out
    private Dictionary<Material, Coroutine> _fadingOutDict = new Dictionary<Material, Coroutine>();

    // Dictionary of all materials/coroutines that are currently fading in
    private Dictionary<Material, Coroutine> _fadingInDict = new Dictionary<Material, Coroutine>();

    [SerializeField, Range(0, 2f)] 
    private float _fadeDuration = 1f;

    [SerializeField, Range(0, 1f)] 
    private float _alphaWhenFaded = 0.1f;

    private InputAction _mousePositionAction;
    private bool _pointerOverUI = false;
    private EventSystem _eventSystem;
    private Transform _transform;
    private Camera _camera;

    private void Start()
    {
        _mousePositionAction = S.I.IM.PC.World.MousePosition;
        _eventSystem = EventSystem.current;
        _transform = transform;
        _camera = Camera.main;
    }

    private void OnEnable()
    {
        PCSelector.OnSelectPC += SelectPC;

/*        SelectedSubstate.OnSelectPC += SelectPC;
        SelectedIdleSubstate.OnDeselectPC += DeselectPC;*/
    }

    private void OnDisable()
    {
        PCSelector.OnSelectPC -= SelectPC;
        
/*        SelectedSubstate.OnSelectPC -= SelectPC;
        SelectedIdleSubstate.OnDeselectPC -= DeselectPC;*/
    }

    private void SelectPC(Transform pcTransform)
    {
        _currentPCTransform = pcTransform;
        // Waiting a frame because SelectPC gets called on the new PC before this. 
//        StartCoroutine(WaitThenSelect(pcTransform));
    }

/*    private IEnumerator WaitThenSelect(Transform pcTransform)
    {
        yield return new WaitForEndOfFrame();

        _currentPCTransform = pcTransform;

        //Debug.Log($"Transparentizer's current PC is {_currentPCTransform.gameObject.name}");
    }*/

    private void DeselectPC()
    {
        _currentPCTransform = null;

        //Debug.Log($"Transparentizer's current PC is null");
    }

    private void Update()
    {
        if (_eventSystem.IsPointerOverGameObject())
        {
            _pointerOverUI = true;
        }
        else
        {
            _pointerOverUI = false;
        }
    }

    private void FixedUpdate()
    {
        RaycastHit[] hits = new RaycastHit[0];

        // Hits from mouse position
        if (!_pointerOverUI)
        {
            hits = Physics.RaycastAll(
                _camera.ScreenPointToRay(_mousePositionAction.ReadValue<Vector2>()),
                1000, 
                _transparentableLayerMask);
        }

        // Hits from currently selected PC
        if (_currentPCTransform != null)
        {
            Vector3 position = _transform.position;
            Vector3 direction = _currentPCTransform.position - position;
            float rayDistance = Vector3.Distance(position, _currentPCTransform.position);

            RaycastHit[] selectedPlayerHits = Physics.RaycastAll(
                position,
                direction,
                rayDistance,
                _transparentableLayerMask);

            // Combine all hits into one array
            hits = selectedPlayerHits.Concat(hits).ToArray();
        }

        // List of all in the way materials, gets remade each fixed update
        List<Material> inTheWayMaterials = new List<Material>();

        foreach (RaycastHit hit in hits)
        {
            List<Material> hitMaterials = GetMaterialsFromHit(hit);

            foreach (Material material in hitMaterials)
            {
                // Adds all in the way materials to list, and starts fading them if they're not already fading/faded
                // Also stops fading-in coroutines and starts fading those materials out
                inTheWayMaterials.Add(AndFadeOutIfNecessary(material));
            }
        }

        // If a material is faded out or fading out but no longer in the way, unfade it
        List<Material> materialsToUnfade = new List<Material>();

        for (int i = _fadedOut.Count - 1; i >= 0; i--)
        {
            if (!inTheWayMaterials.Contains(_fadedOut[i]))
            {
                // If a material is FADED out but no longer in the way, unfade it
                materialsToUnfade.Add(AndUnfade(_fadedOut[i]));
                _fadedOut.Remove(_fadedOut[i]);
            }
        }

        // Why a list of kvps instead of a dictionary? 
        List<KeyValuePair<Material, Coroutine>> itemsToRemove = new List<KeyValuePair<Material, Coroutine>>();
        foreach (KeyValuePair<Material, Coroutine> kvp in _fadingOutDict)
        {
            if (!inTheWayMaterials.Contains(kvp.Key))
            {
                // If a material is FADING out but no longer in the way, unfade it
                itemsToRemove.Add(kvp);
            }
        }
        foreach (KeyValuePair<Material, Coroutine> kvp in itemsToRemove)
        {
            StopCoroutine(kvp.Value);
            materialsToUnfade.Add(AndUnfade(kvp.Key));
            _fadingOutDict.Remove(kvp.Key);
        }
    }

    private List<Material> GetMaterialsFromHit(RaycastHit hit)
    {
        List<Material> materials = new List<Material>();
        List<MeshRenderer> meshRenderers = new List<MeshRenderer>();

        meshRenderers.AddRange(hit.collider.GetComponents<MeshRenderer>());
        // Uncomment below line if using objects with colliders on grandchildren
        //meshRenderers.AddRange(hit.collider.GetComponentsInChildren<MeshRenderer>());

        if (meshRenderers.Count > 0)
        {
            foreach (MeshRenderer meshRenderer in meshRenderers)
            {
                materials.AddRange(meshRenderer.materials);
            }
        }

        return materials;
    }

    private Material AndFadeOutIfNecessary(Material material)
    {
        // If material isn't faded out or fading out, ...
        if (!_fadedOut.Contains(material) && !_fadingOutDict.ContainsKey(material))
        {
            // If material is fading in, stop that coroutine and remove it from dictionary
            if (_fadingInDict.ContainsKey(material))
            {
                StopCoroutine(_fadingInDict[material]);
                _fadingInDict.Remove(material);
            }

            // Start fade out coroutine
            Color fadeColor = new Color(
                material.color.r, material.color.g, material.color.b, _alphaWhenFaded);

            Coroutine coroutine = StartCoroutine(Fade(material, fadeColor, _fadeDuration));

            // Add to fadingOutDict, then add to faded list when coroutine done
            _fadingOutDict.Add(material, coroutine);

            return material;
        }
        else
        {
            return material;
        }
    }

    private Material AndUnfade(Material material)
    {
        Color originalColor = new Color(
            material.color.r, material.color.g, material.color.b, 1f);

        Coroutine coroutine = StartCoroutine(Fade(material, originalColor, _fadeDuration));

        // Add to fadingInDict. Remove from fadingInDict when coroutine done
        _fadingInDict.Add(material, coroutine);

        return material;
    }

    private IEnumerator Fade(Material material, Color fadedColor, float duration)
    {
        float time = 0f;
        Color regularColor = material.color;

        while (time < duration)
        {
            // TODO: Fix this lerp. Use idea from CameraControllerFollower comments
            material.color = Color.Lerp(material.color, fadedColor, time);
            time += Time.deltaTime;
            yield return null;
        }

        material.color = fadedColor;

        // If material is fading out (not in), ...
        if (_fadingOutDict.ContainsKey(material))
        {
            // Remove from fadingOutDict
            _fadingOutDict.Remove(material);

            // Add to faded list
            _fadedOut.Add(material);
        }
        else if (_fadingInDict.ContainsKey(material))
        {
            // Remove from fadingInDict
            _fadingInDict.Remove(material);
        }
    }
}