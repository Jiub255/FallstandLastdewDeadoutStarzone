using System;
using System.Collections;
using UnityEngine;

public class NotSelectedSubstate : MonoBehaviour
{
   // public static event Action OnDeselectPC;

    private void OnEnable()
    {
        // Send Transparentizer and PCSelector the signal to set current PC transform to null. 
      //  OnDeselectPC?.Invoke();

        // Deactivate selected icon. 
        StartCoroutine(WaitThenDeactivateIcon());
        //transform.root.GetComponentInChildren<SelectedPCIcon>(true).DeactivateIcon();
    }

    // TODO: Is it necessary to wait a frame? 
    IEnumerator WaitThenDeactivateIcon()
    {
        yield return new WaitForEndOfFrame();

        transform.root.GetComponentInChildren<SelectedPCIcon>(true).DeactivateIcon();
    }
}