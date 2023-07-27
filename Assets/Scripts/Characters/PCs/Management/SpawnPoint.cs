using System;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    /// <summary>
    /// Heard by GameManager, instantiates PCs, gets their instance references in SOPCData,
    /// and populates PCManager's PCController dictionary. 
    /// </summary>
    public static event Action<Vector3> OnSceneStart;

/*    [SerializeField]
    private SOCurrentTeam _currentTeamSO;

    private SOCurrentTeam CurrentTeamSO { get { return _currentTeamSO; } }*/

    private void Start/*Awake*/()
    {
        OnSceneStart?.Invoke(transform.position);
        // Where to put instantiated PCs?
        //     Scavenging: Have a spawn area in each scavenge location and just spawn them in a bunch in the center.
        //         Make area big enough for max amount of PCs
        //     Home Base: Spawn next to random buildings that PCs can interact with in home scene?
        //         Maybe include a spawn point on each building?
        //         Or just choose random spots on the home base map from the pathfinding grid,
        //             and make sure they're at least x units from each other. 

        // For now, just instantiating them in a line starting at the spawn point. 

/*        if (_currentTeamSO.HomeSOPCSList.Count > 0)
        {
            Transform spawnPointTransform = transform;
            for (int i = 0; i < _currentTeamSO.HomeSOPCSList.Count; i++)
            {
                _currentTeamSO.HomeSOPCSList[i].PCInstance = Instantiate(
                    _currentTeamSO.HomeSOPCSList[i].PCPrefab,
                    new Vector3(3 * i, 0f, 0f) + spawnPointTransform.position,
                    Quaternion.identity);
            }

            // Set first instantiated as CurrentMenuSOPC. 
            _currentTeamSO.CurrentMenuSOPC = _currentTeamSO.HomeSOPCSList[0];
        }*/
    }
}