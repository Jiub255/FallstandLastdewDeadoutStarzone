using System.Collections;
using UnityEngine;

public class NotSelectedSubstate : MonoBehaviour
{
    private void OnEnable()
    {
        // Deactivate selected icon. 
        StartCoroutine(WaitThenDeactivateIcon());
    }

    // TODO: Is it necessary to wait a frame? 
    IEnumerator WaitThenDeactivateIcon()
    {
        yield return new WaitForEndOfFrame();

        transform.parent.parent.parent.GetComponentInChildren<SelectedPCIcon>(true).DeactivateIcon();
    }
}