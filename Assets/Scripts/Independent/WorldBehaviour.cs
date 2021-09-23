using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBehaviour : MonoBehaviour
{
    public float voidLevel = -100;

    // Update is called once per frame
    void FixedUpdate()
    {
        if(transform.position.y < voidLevel)
        {
            Destroy(gameObject);
        }
    }
}
