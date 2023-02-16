using System.Collections;
using UnityEngine;

public class NotSelectedSubstate : MonoBehaviour
{
    private void OnEnable()
    {
        // Deactivate selected icon. 
        StartCoroutine(WaitThenDeactivateIcon());
    }

    IEnumerator WaitThenDeactivateIcon()
    {
        yield return new WaitForEndOfFrame();

        transform.parent.parent.parent.GetComponentInChildren<SelectedPCIcon>().DeactivateIcon();
    }
}