using UnityEngine;
using UnityEditor;

public class CleanMissingScripts
{
    [MenuItem("Tools/Clean Missing Scripts In Scene")]
    static void CleanScene()
    {
        GameObject[] objects = Object.FindObjectsByType<GameObject>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None
        );

        int removed = 0;

        foreach (GameObject go in objects)
        {
            removed += GameObjectUtility.RemoveMonoBehavioursWithMissingScript(go);
        }

        Debug.Log("Missing Scripts eliminados: " + removed);
    }
}