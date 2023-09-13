using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Building/SOBuildingDatabase", fileName = "Building Database SO")]
public class SOBuildingDatabase : ScriptableObject
{
    [SerializeField]
    private List<SOBuilding> _buildings;

    public List<SOBuilding> Buildings { get { return _buildings; } }

    public void GetAllBuildings()
    {
        Buildings.Clear();

        // TODO - Using UnityEditor here, so need to find another way, or do this before building the game. 
        // Maybe run this method manually right before building? 
        string[] assetNames = AssetDatabase.FindAssets(
            "t:SOBuilding",
            new[] { "Assets/SOs/Building/Buildings" });

        foreach (string SOName in assetNames)
        {
            string SOpath = AssetDatabase.GUIDToAssetPath(SOName);
            SOBuilding buildingSO = AssetDatabase.LoadAssetAtPath<SOBuilding>(SOpath);
            Buildings.Add(buildingSO);
        }

        Debug.Log($"Number of buildings: {Buildings.Count}");
    }
}

[CustomEditor(typeof(SOBuildingDatabase))]
public class SOBuildingDatabaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SOBuildingDatabase database = (SOBuildingDatabase)target;

        if (GUILayout.Button("Get Buildings"))
        {
            database.GetAllBuildings();
        }

        DrawDefaultInspector();
    }
}