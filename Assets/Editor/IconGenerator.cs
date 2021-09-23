using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEditor.Experimental.SceneManagement;
using System.IO;

public class IconGenerator : EditorWindow
{
    public GameObject[] objs;
    public int resolution = 128;

    [MenuItem("Window/Icon Generator")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(IconGenerator));
    }

    void OnGUI()
    {
        resolution = EditorGUILayout.IntField(new GUIContent("Resolution"), resolution);

        if (objs == null) objs = new GameObject[0];
        // "target" can be any class derrived from ScriptableObject 
        // (could be EditorWindow, MonoBehaviour, etc)
        ScriptableObject target = this;
        SerializedObject so = new SerializedObject(target);
        SerializedProperty objsProperty = so.FindProperty("objs");

        if (GUILayout.Button("Generate Icon"))
        {
            if(resolution < 16)
            {
                Debug.LogError("Icon Generator's resolution value must be greater than 16.");
                return;
            }
            foreach (GameObject obj in objs)
            {
                string myPath = AssetDatabase.GetAssetPath(obj);
                AssetDatabase.OpenAsset(obj);

                PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();

                GameObject root = prefabStage.scene.GetRootGameObjects()[0];

                root.transform.position = Vector3.zero;

                Renderer[] renderers = root.GetComponentsInChildren<Renderer>();

                Bounds bounds = new Bounds();

                foreach (var renderer in renderers)
                {
                    bounds.Encapsulate(renderer.bounds);
                }

                GameObject lightObj = new GameObject();
                EditorSceneManager.MoveGameObjectToScene(lightObj, prefabStage.scene);
                Light light = lightObj.AddComponent<Light>();
                light.type = LightType.Directional;
                lightObj.transform.rotation = Quaternion.LookRotation(Vector3.down, Vector3.up);


                GameObject cameraObj = new GameObject();
                EditorSceneManager.MoveGameObjectToScene(cameraObj, prefabStage.scene);
                Camera camera = cameraObj.AddComponent<Camera>();
                camera.gameObject.name = "Camera";
                camera.transform.SetParent(root.transform);
                camera.fieldOfView = 20;
                camera.nearClipPlane = 0.01f;
                camera.clearFlags = CameraClearFlags.Nothing;
                camera.scene = prefabStage.scene;
                float distance = 1 / Mathf.Tan(Mathf.Deg2Rad * 15) * (bounds.size/2).magnitude;
                camera.transform.position = new Vector3(distance, distance, distance);

                Vector3 offset = bounds.center;
                camera.transform.position += offset;
                camera.transform.LookAt(bounds.center);

                RenderTexture rt = new RenderTexture(resolution, resolution, 1, RenderTextureFormat.ARGB32);
                rt.Create();

                camera.targetTexture = rt;
                camera.Render();

                byte[] bytes = toTexture2D(rt).EncodeToPNG();
                File.WriteAllBytes($"{Directory.GetParent(myPath)}/{root.name}.png", bytes);

                rt.Release();

                DestroyImmediate(lightObj);
                DestroyImmediate(cameraObj);

            }
            AssetDatabase.Refresh();
        }

        if (GUILayout.Button("Empty List"))
        {
            objs = new GameObject[0];
        }

        EditorGUILayout.PropertyField(objsProperty, true); // True means show children
        so.ApplyModifiedProperties(); // Remember to apply modified properties
    }
    Texture2D toTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.ARGB32, false);
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }
}