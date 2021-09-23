using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ProbePlacer : MonoBehaviour
{
    private ARRaycastManager rayManager;
    private AREnvironmentProbeManager probeManager;
    [SerializeField] private GameObject environmentProbe;

    void Start()
    {
        // get the components
        rayManager = FindObjectOfType<ARRaycastManager>();
        probeManager = FindObjectOfType<AREnvironmentProbeManager>();
    }

    void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                // shoot a raycast from the center of the screen
                List<ARRaycastHit> hits = new List<ARRaycastHit>();
                rayManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.Planes);

                // if we hit an AR plane surface, update the position and rotation
                if (hits.Count > 0)
                {
                    Debug.Log(probeManager.enabled);
                    Debug.Log(probeManager.subsystem == null);
                }
            }
        }
    }

    XREnvironmentProbeSubsystem EnvironmentProbeSubsystem()
    {
        // Get all available plane subsystems
        var descriptors = new List<XREnvironmentProbeSubsystemDescriptor>();
        SubsystemManager.GetSubsystemDescriptors(descriptors);

        // Find one that supports boundary vertices
        foreach (var descriptor in descriptors)
        {
            if (descriptor.supportsManualPlacement)
            {
                // Create this plane subsystem
                return descriptor.Create();
            }
        }

        return null;
    }
}
