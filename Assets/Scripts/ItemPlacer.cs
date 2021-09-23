using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ItemPlacer : MonoBehaviour
{
    private ARRaycastManager rayManager;
    public SelectionBehaviour selection;

    [SerializeField] Button placeBtn;
    [SerializeField] Button deleteAllBtn;
    [SerializeField] Button undoBtn;

    private List<GameObject> placedObjects = new List<GameObject>();
    private Stack<GameObject> placedStack = new Stack<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        // get the components
        rayManager = FindObjectOfType<ARRaycastManager>();
        placeBtn.onClick.AddListener(PlaceObject);
        deleteAllBtn.onClick.AddListener(DeleteAllObjects);
        undoBtn.onClick.AddListener(Undo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Undo()
    {
        Destroy(placedStack.Pop());
    }

    private void DeleteAllObjects()
    {
        foreach (GameObject go in placedObjects)
        {
            Destroy(go);
        }
        placedObjects = new List<GameObject>();
    }

    private void PlaceObject()
    {
        // shoot a raycast from the center of the screen
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        rayManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.PlaneWithinPolygon);

        // if we hit an AR plane surface, update the position and rotation
        if (hits.Count > 0)
        {
            GameObject newObj = Instantiate(selection.GetSelected());

            newObj.transform.position = hits[0].pose.position;
            newObj.transform.rotation = hits[0].pose.rotation;

            newObj.AddComponent<WorldBehaviour>();

            placedObjects.Add(newObj);
            placedStack.Push(newObj);
        }
    }
}
