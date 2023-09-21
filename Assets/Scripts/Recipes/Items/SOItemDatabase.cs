using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/SOItemDatabase", fileName = "Item Database SO")]
public class SOItemDatabase : ScriptableObject
{
	[SerializeField]
	private List<SOItem> _items;

	public List<SOItem> Items { get { return _items; } }

    public void GetAllItems()
    {
        Items.Clear();

        // TODO - Using UnityEditor here, so need to find another way, or do this before building the game. 
        // Maybe run this method manually right before building? 
        string[] assetNames = AssetDatabase.FindAssets(
            "t:SOItem",
            new[] { "Assets/SOs/Items" });

        foreach (string SOName in assetNames)
        {
            string SOpath = AssetDatabase.GUIDToAssetPath(SOName);
            SOItem itemSO = AssetDatabase.LoadAssetAtPath<SOItem>(SOpath);
            Items.Add(itemSO);
        }

        Debug.Log($"Number of items: {Items.Count}");
    }
}

[CustomEditor(typeof(SOItemDatabase))]
public class SOItemDatabaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SOItemDatabase database = (SOItemDatabase)target;

        if (GUILayout.Button("Get Items"))
        {
            database.GetAllItems();
        }
    }
}