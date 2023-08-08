using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mb : MonoBehaviour
{
    [SerializeField]
    private mb2 _mb2;

    private void Awake()
    {
        StartCoroutine(Toggle());
    }

    private IEnumerator Toggle()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            _mb2.enabled = true;
            yield return new WaitForSeconds(1);
            _mb2.enabled = false;
        }
    }
}