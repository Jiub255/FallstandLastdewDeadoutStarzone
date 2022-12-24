using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Put this on camera
public class JUNKMouseTransparentizer : MonoBehaviour
{
    /*[SerializeField]
    private LayerMask transparentable;

    private List<Material> fadedOut = new List<Material>();

    [SerializeField, Range(0, 2f)] private float fadeDuration = 1f;

    [SerializeField, Range(0, 1f)] private float minimalOpacity = 0.1f;

    // NEW INPUT SYSTEM STUFF
    private PlayerInput playerInput;

    private InputAction mousePositionAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        mousePositionAction = playerInput.actions["MousePosition"];
    }

    private void FixedUpdate()
    {
        RaycastHit[] hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(
            mousePositionAction.ReadValue<Vector2>()), 100, transparentable);

        List<Material> inTheWayMaterials = new List<Material>();
        foreach (RaycastHit hit in hits)
        {
            Debug.Log(hit.collider.name + " is colliding with ray");

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

            foreach (Material material in materials)
            {
                if (!fadedOut.Contains(material))
                {
                    fadedOut.Add(material);

                    Color fadeColor = new Color(
                        material.color.r, material.color.g, material.color.b, minimalOpacity);
                    StartCoroutine(Fade(material, fadeColor, fadeDuration));

                    inTheWayMaterials.Add(material);
                }
                else
                {
                    inTheWayMaterials.Add(material);
                }
            }
        }

        List<Material> materialsToDelete = new List<Material>();

        foreach (Material material in fadedOut)
        {
            if (!inTheWayMaterials.Contains(material))
            {
                Color originalColor = new Color(
                    material.color.r, material.color.g, material.color.b, 1f);
                StartCoroutine(Fade(material, originalColor, fadeDuration));

                materialsToDelete.Add(material);
            }
        }

        foreach (Material material in materialsToDelete)
        {
            fadedOut.Remove(material);
        }
    }

    private IEnumerator Fade(Material material, Color fadedColor, float duration)
    {
        Debug.Log("Fade coroutine started");

        float time = 0f;
        Color regularColor = material.color;

        while (time < duration)
        {
            material.color = Color.Lerp(material.color, fadedColor, time);
            time += Time.deltaTime;
            yield return null;
        }

        material.color = fadedColor;
    }*/
}