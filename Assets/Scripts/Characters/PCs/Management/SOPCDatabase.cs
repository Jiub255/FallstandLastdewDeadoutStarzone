using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "PC/SOPCDatabase", fileName = "PC Database SO")]
public class SOPCDatabase : ScriptableObject
{
    [SerializeField]
    private List<SOPCData> _pcDataSOs;

    public List<SOPCData> PCDataSOs { get { return _pcDataSOs; } }

    public void GetAllPCs()
    {
        PCDataSOs.Clear();

        // TODO - Using UnityEditor here, so need to find another way, or do this before building the game. 
        // Maybe run this method manually right before building? 
        string[] assetNames = AssetDatabase.FindAssets(
            "t:SOPCData",
            new[] { "Assets/SOs/PCs" });

        foreach (string SOName in assetNames)
        {
            string SOpath = AssetDatabase.GUIDToAssetPath(SOName);
            SOPCData pcDataSO = AssetDatabase.LoadAssetAtPath<SOPCData>(SOpath);
            PCDataSOs.Add(pcDataSO);
        }

        Debug.Log($"Number of PCs: {PCDataSOs.Count}");
    }
}

[CustomEditor(typeof(SOPCDatabase))]
public class SOPCDatabaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SOPCDatabase database = (SOPCDatabase)target;

        if (GUILayout.Button("Get PCs"))
        {
            database.GetAllPCs();
        }

        DrawDefaultInspector();
    }
}