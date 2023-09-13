using UnityEngine;

[CreateAssetMenu(menuName = "Data/SOSaveSystemData", fileName = "Save System Data SO")]
public class SOSaveSystemData : ScriptableObject
{
    [SerializeField]
    private string _fileName = "";

    [SerializeField]
    private float _autoSaveTimeSeconds = 60f;

    public string FileName { get { return _fileName; } }
    public float AutoSaveTimeSeconds { get { return _autoSaveTimeSeconds; } }
}