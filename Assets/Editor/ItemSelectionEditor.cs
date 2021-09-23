using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(ItemSelection))]
public class ItemSelectionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // If user clicks generate
        if (GUILayout.Button(new GUIContent("Generate Items")))
        {
            // Popup a Dialog to user, If user press Yes, then generate, if cancel, nothing happens.
            if (EditorUtility.DisplayDialog("Regenerate all objects?", "Are you sure to DELETE all objects before generating?", "Yes", "Cancel"))
            {
                // Cast target to ItemSelection for later use
                ItemSelection t = (ItemSelection)target;

                t.RemoveAllContent(); // Remove all generated content before generating new gameobjects
                List<GameObject> prefabs = t.GetPrefabs(); // Get prefabs to generate
                GameObject panelPrefab = t.GetPanelPrefab(); // Get panel prefab
                Transform content = t.GetContent(); // Get scroll view's content transform

                // For each prefabs
                for (int i = 0; i < prefabs.Count; i++)
                {
                    // Get current prefab
                    GameObject item = prefabs[i];

                    // Path of gameobject prefab
                    string path = AssetDatabase.GetAssetPath(item);

                    // Path of gameobject prefab without extension
                    string fileNameWithoutExt = Path.GetFileNameWithoutExtension(path);

                    // Path of gameobject's icon, file name must be the same
                    string iconPath = $"{Directory.GetParent(path)}/{fileNameWithoutExt}.png";

                    // If given gameobject's icon exist, use icon, if not, use default prefab icon
                    Texture2D myTexture;
                    if (File.Exists(iconPath))
                    {
                        myTexture = new Texture2D(1, 1);
                        myTexture.LoadImage(File.ReadAllBytes(iconPath));
                    }
                    else
                    {
                        myTexture = PrefabUtility.GetIconForGameObject(item);
                    }

                    // Generate panel with button on it
                    GameObject newPanel = Instantiate(panelPrefab);
                    newPanel.transform.SetParent(content); // Set Parent to content
                    newPanel.name = fileNameWithoutExt; // Set name
                    newPanel.GetComponent<RawImage>().texture = myTexture; // Set texture

                    // Add listener to generate button
                    newPanel.GetComponent<Button>().onClick.AddListener(() => {
                        t.SetSelectedIndex(i);
                    });
                }
            }
        }
    }
}
