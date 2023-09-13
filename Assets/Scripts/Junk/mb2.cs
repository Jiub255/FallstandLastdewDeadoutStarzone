using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mb2 : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("Awake");
    }
    private void OnEnable()
    {
        Debug.Log("OnEnable");
    }
    private void Start()
    {
        Debug.Log("Start");
    }
    private void OnDisable()
    {
        Debug.Log("OnDisable");
    }
}