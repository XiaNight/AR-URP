using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleActive : MonoBehaviour
{
    public List<GameObject> targets;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Toggle()
    {
        foreach (GameObject target in targets)
        {
            if(target != null)
            {
                target.SetActive(!target.activeInHierarchy);
            }
        }
    }
}