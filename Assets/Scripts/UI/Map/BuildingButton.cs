using UnityEngine;

// Controls switching to different scavenging scenes by clicking on the buildings on the map. 
// Put one on each button. 
public class BuildingButton : MonoBehaviour
{
    public static event System.Action<GameObject> OnClickMapButton;

    [SerializeField]
    private GameObject _levelPrefab;

    // Just load a blank scavenging scene, then instantiate the actual level's prefab. 
    public void LoadScene()
    {
        OnClickMapButton?.Invoke(_levelPrefab);
//        S.I.SceneTransitionManager.LoadScavengingScene(_levelPrefab);
    }
}