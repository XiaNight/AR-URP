using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugInformation : MonoBehaviour
{
    private GUIStyle guiStyle = new GUIStyle(); //create a new variable

    // Start is called before the first frame update
    void Start()
    {
        guiStyle.fontSize = 30;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, Screen.width / 2, 50), new GUIContent($"FPS: {1 / Time.deltaTime}"), guiStyle);
    }
}
