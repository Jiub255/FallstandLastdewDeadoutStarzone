using UnityEngine;

public class PCInstantiator : MonoBehaviour
{
    [SerializeField]
    private SOListSOPC _pcSOsSO;

    private void Awake()
    {
        // Where to put instantiated PCs?
        //     Scavenging: Have a spawn area in each scavenge location and just spawn them in a bunch in the center.
        //         Make area big enough for max amount of PCs
        //     Home Base: Spawn next to random buildings that PCs can interact with in home scene?
        //         Maybe include a spawn point on each building?
        //         Or just choose random spots on the home base map from the pathfinding grid,
        //             and make sure they're at least x units from each other. 

        // For now, just instantiating them in a line starting at the spawn point. 
        for (int i = 0; i < _pcSOsSO.SOPCs.Count; i++)
        {
            _pcSOsSO.SOPCs[i].PCInstance = Instantiate(
                _pcSOsSO.SOPCs[i].PCPrefab,
                new Vector3(3 * i, 0f, 0f) + transform.position,
                Quaternion.identity);
        }
    }
}