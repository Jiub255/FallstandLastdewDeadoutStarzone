using UnityEngine;

public static class StateSwitcher
{
	public static void Switch(GameObject from, GameObject to)
    {
        Debug.Log($"[StateSwitcher] from {from.name} to {to.name}");

        // Activate Selected substate if PC is currently selected. 
        if (from.GetComponentInChildren<SelectedSubstate>(true).gameObject.activeSelf)
        {
            to.GetComponentInChildren<SelectedSubstate>(true).gameObject.SetActive(true);
            to.GetComponentInChildren<NotSelectedSubstate>(true).gameObject.SetActive(false);
        }
        // Activate NotSelected substate if PC is not currently selected. 
        else if (from.GetComponentInChildren<NotSelectedSubstate>(true).gameObject.activeSelf)
        {
            to.GetComponentInChildren<NotSelectedSubstate>(true).gameObject.SetActive(true);
            to.GetComponentInChildren<SelectedSubstate>(true).gameObject.SetActive(false);
        }

        // Activate new state. 
        to.SetActive(true);

        // Deactivate old state. 
        from.SetActive(false);
    }
}