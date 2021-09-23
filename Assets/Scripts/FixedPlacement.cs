using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedPlacement : MonoBehaviour
{
    public Vector3 facingDirection;

    private Transform arOrigin;

    // Start is called before the first frame update
    void Start()
    {
        arOrigin = GameObject.FindGameObjectWithTag("AROrigin").transform;
        transform.SetParent(arOrigin);
        transform.rotation = Quaternion.LookRotation(facingDirection, Vector3.up);
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(facingDirection, Vector3.up);
    }
}
