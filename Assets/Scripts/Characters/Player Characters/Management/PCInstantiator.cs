using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Just for testing for now. Will have this in a SceneManager or something similar.
public class PCInstantiator : MonoBehaviour
{
	[SerializeField]
	private PCSOListSO availablePCsSO;

    private void Awake()
    {
        // Where to put instantiated PCs?
        //     Have a spawn area in each scavenge location and just spawn them in a bunch in the center.
        //         Make area big enough for max amount of PCs
        //     Spawn next to random buildings that PCs can interact with in home scene.
        //         Maybe include a spawn point on each building?

        for (int i = 0; i < availablePCsSO.PCItemSOs.Count; i++)
        {
           // Debug.Log("Instantiating PC #" + (i + 1).ToString());

            availablePCsSO.PCItemSOs[i].PCInstance = Instantiate(
                availablePCsSO.PCItemSOs[i].PCPrefab, new Vector3(3 * i, 0f, 0f), Quaternion.identity);

           // Debug.Log(availablePCsSO.PCItemSOs[i].PCInstance.name);
        }
    }
}