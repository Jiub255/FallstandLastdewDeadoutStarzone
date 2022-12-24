using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

// Put this on camera
public class Transparentizer : MonoBehaviour
{
    [SerializeField]
    private LayerMask transparentableLayer;

    private Transform currentPlayerCharacterTransform;

    private List<Material> fadedOut = new List<Material>();

    [Tooltip("Length of the fade")]
    [SerializeField, Range(0, 2f)] private float fadeDuration = 1f;

    [Tooltip("Opacity of the object when fully faded")]
    [SerializeField, Range(0, 1f)] private float minimalOpacity = 0.1f;

    // NEW INPUT SYSTEM STUFF
/*    private PlayerInput playerInput;

    private InputAction mousePositionAction;*/

    private void Awake()
    {
/*        playerInput = GetComponent<PlayerInput>();
        mousePositionAction = playerInput.actions["MousePosition"];*/
    }

    private void OnEnable()
    {
        PlayerCharacterController.onChangedSelectedCharacter += ChangeCurrentCharacter;
    }

    private void OnDisable()
    {
        PlayerCharacterController.onChangedSelectedCharacter -= ChangeCurrentCharacter;
    }

    private void ChangeCurrentCharacter(Transform newCharacterTransform)
    {
        currentPlayerCharacterTransform = newCharacterTransform;
    }

    private void FixedUpdate()
    {
        // Hits from mouse position
        RaycastHit[] hits = Physics.RaycastAll(
            Camera.main.ScreenPointToRay(MasterSingleton.Instance.InputManager.mousePositionAction.ReadValue<Vector2>()),
            100, 
            transparentableLayer);

        if (currentPlayerCharacterTransform != null)
        {
            // Hits from currently selected PC
            Vector3 position = transform.position;
            Vector3 direction = currentPlayerCharacterTransform.position - position;
            float rayDistance = Vector3.Distance(position, currentPlayerCharacterTransform.position);

            RaycastHit[] selectedPlayerHits = Physics.RaycastAll(
                position,
                direction,
                rayDistance,
                transparentableLayer);

            // Combine all hits into one array
            hits = selectedPlayerHits.Concat(hits).ToArray();
        }

        // List gets remade each fixed update
        List<Material> inTheWayMaterials = new List<Material>();

        foreach (RaycastHit hit in hits)
        {
            // Debug.Log(hit.collider.name + " is colliding with ray");

            List<Material> hitMaterials = GetMaterialsFromHit(hit);

            foreach (Material material in hitMaterials)
            {
                inTheWayMaterials.Add(AndFadeIfNecessary(material));
            }
        }

        List<Material> materialsToUnfade = new List<Material>();

        foreach (Material material in fadedOut)
        {
            if (!inTheWayMaterials.Contains(material))
            {
                materialsToUnfade.Add(AndUnfade(material));
            }
        }

        foreach (Material material in materialsToUnfade)
        {
            fadedOut.Remove(material);
        }
    }

    private List<Material> GetMaterialsFromHit(RaycastHit hit)
    {
        List<Material> materials = new List<Material>();
        List<MeshRenderer> meshRenderers = new List<MeshRenderer>();

        meshRenderers.AddRange(hit.collider.GetComponents<MeshRenderer>());
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

    private Material AndFadeIfNecessary(Material material)
    {
        if (!fadedOut.Contains(material))
        {
            fadedOut.Add(material);

            Color fadeColor = new Color(
                material.color.r, material.color.g, material.color.b, minimalOpacity);
            StartCoroutine(Fade(material, fadeColor, fadeDuration));

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
        StartCoroutine(Fade(material, originalColor, fadeDuration));

        return material;
    }

    private IEnumerator Fade(Material material, Color fadedColor, float duration)
    {
       // Debug.Log("Fade coroutine started");

        float time = 0f;
        Color regularColor = material.color;

        while (time < duration)
        {
            material.color = Color.Lerp(material.color, fadedColor, time);
            time += Time.deltaTime;
            yield return null;
        }

        material.color = fadedColor;
    }
}