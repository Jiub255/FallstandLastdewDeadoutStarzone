using System;
using UnityEngine;

// TODO: Use PCItemSO instead
[CreateAssetMenu(fileName = "New PC Item SO", menuName = "Scriptable Object/Player Characters/PC Item SO")]
public class PCItemSO : ItemSO
{
    public static event Action<PCItemSO> OnSelectPC;

    public GameObject PCPrefab;

    public string Name;

    // Gets set in game while instantiating the scene's PCs.
    // Do in SceneManager?
    /*    foreach (PCItemSO pCItemSO in availablePCsSO.PCItemSOs)
        {
            pCItemSO.PCInstance = Instantiate(pCItemSO.PCPrefab);
        }*/
    [HideInInspector]
    public GameObject PCInstance;

    public override void Use()
    {
        //_selectedPCSO.PCSO = this;

        // PCSelector hears this. Selects this PC for now. Center camera on double click later. 
        OnSelectPC?.Invoke(this);

        base.Use();
    }
}