using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

static class SOResetter
{
    [InitializeOnLoadMethod]
    static void RegisterResets()
    {
        EditorApplication.playModeStateChanged += ResetSOsWithIResettable;
    }

    static void ResetSOsWithIResettable(PlayModeStateChange change)
    {
        if (change == PlayModeStateChange.ExitingPlayMode)
        {
            ScriptableObject[] assets = FindAssets<ScriptableObject>();
            foreach (ScriptableObject so in assets)
            {
                if (so is IResettable)
                {
                    (so as IResettable).ResetOnExitPlayMode();
                }
            }
        }
    }

    static T[] FindAssets<T>() where T : Object
    {
        string[] guids = AssetDatabase.FindAssets($"t:{typeof(T)}");
        T[] assets = new T[guids.Length];
        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            assets[i] = AssetDatabase.LoadAssetAtPath<T>(path);
        }
        return assets;
    }
}
#endif