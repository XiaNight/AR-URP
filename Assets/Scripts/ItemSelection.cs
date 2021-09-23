using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelection : SelectionBehaviour
{
    [SerializeField] Button leftBtn;
    [SerializeField] Button rightBtn;
    [SerializeField] Transform content;

    [SerializeField] List<GameObject> prefabs;
    [SerializeField] GameObject panelPrefab;

    [SerializeField] GameObject selectedObject;

    // Start is called before the first frame update
    void Start()
    {
        SetContentButtonListeners();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetContentButtonListeners()
    {
        for (int i = 0; i < content.childCount; i++)
        {
            Button btn = content.GetChild(i).GetComponent<Button>();
            int tempI = i;
            btn.onClick.AddListener(delegate { SetSelectedIndex(tempI); });
        }
    }

    public override void SetSelectedIndex(int index)
    {
        if (0 <= index && index < prefabs.Count)
        {
            selectedObject = prefabs[index];
        }
        else
        {
            Debug.LogError($"Index out of range, index: {index}, content size: {prefabs.Count}");
        }
    }

    public void RemoveAllContent()
    {
        for (int i = content.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(content.GetChild(i).gameObject);
        }
    }

    public override GameObject GetSelected()
    {
        return selectedObject;
    }

    public Transform GetContent()
    {
        return content;
    }

    public List<GameObject> GetPrefabs()
    {
        return prefabs;
    }

    public GameObject GetPanelPrefab()
    {
        return panelPrefab;
    }
}