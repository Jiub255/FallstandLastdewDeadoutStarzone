using UnityEngine;

// Controls switching to different scavenging scenes by clicking on the buildings on the map. 
// Put one on each button. 
public class BuildingButton : MonoBehaviour
{
    [SerializeField]
    private GameObject _levelPrefab;

    // Just load a blank scavenging scene, then instantiate the actual level's prefab. 
    public void LoadScene()
    {
//        S.I.SceneTransitionManager.LoadScavengingScene(_levelPrefab);
    }
}