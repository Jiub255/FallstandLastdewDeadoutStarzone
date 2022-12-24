using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class JUNKTransparentizer : MonoBehaviour
{
    // Why doesn't this work when private?
    // Specifically, this line gets a null reference exception:
    // materials.Add(child.GetComponent<MeshRenderer>().material);
    /*public List<Material> materials;

    private void Awake()
    {
        if (GetComponent<MeshRenderer>() != null)
        {
            materials.Add(GetComponent<MeshRenderer>().material);
        }

        // Maybe add grandchildren? Or even further down the hierarchy?
        foreach (Transform child in transform)
        {
            Debug.Log(child.name);

            if (child.GetComponent<MeshRenderer>() != null)
            {
                Debug.Log(child.name + " has a MeshRenderer");

                materials.Add(child.GetComponent<MeshRenderer>().material);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(transform.name + " Collided with player");

            foreach (Material material in materials)
            {
                // Instantly change alpha to 0.
                // TODO: Do a fade later.
                material.color = new Color(material.color.r, material.color.g, material.color.b, 0f);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (Material material in materials)
            {
                // Instantly change alpha back to 1.
                // TODO: Do a fade later.
                material.color = new Color(material.color.r, material.color.g, material.color.b, 1f);
            }
        }
    }*/
}