using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class HideFlagDontSaveCleaner : EditorWindow
{
    [MenuItem("Window/Other/Clean Hide Flag Don't Saves")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(HideFlagDontSaveCleaner));
    }

    private List<GameObject> results = new List<GameObject>();

    void OnGUI()
    {
        if (GUILayout.Button("Search Hide Flags"))
        {
            GameObject[] gameObjects = FindObjectsOfType<GameObject>();
            if(EditorUtility.DisplayDialog("Proceed searching?", $"Found {gameObjects.Length} Gameobjects", "Proceed", "Cancel"))
            {
                foreach (GameObject go in gameObjects)
                {
                    if(go.hideFlags == HideFlags.DontSave)
                    {
                        results.Add(go);
                    }
                }
                if(EditorUtility.DisplayDialog("Search finished", $"Found {results.Count} of Gameobjects with HideFlag of DontSave", "OK"))
                {

                }
            }
        }
    }
}
