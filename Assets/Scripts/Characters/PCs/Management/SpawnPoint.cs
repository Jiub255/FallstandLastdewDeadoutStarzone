using System;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    /// <summary>
    /// Heard by PCManager, instantiates PCs, gets their instance references in SOPCData, and makes PCController. 
    /// </summary>
    public static event Action<Vector3> OnSceneStart;

    private void Start/*Awake*/()
    {
        OnSceneStart?.Invoke(transform.position);
    }
}