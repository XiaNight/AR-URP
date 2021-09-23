using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

public class StableTracking : MonoBehaviour
{
    [SerializeField] int quality = 5;

    [SerializeField]
    private GameObject[] trackedObjects;

    private Dictionary<ARTrackedImage, TrackedImageData> history = new Dictionary<ARTrackedImage, TrackedImageData>();
    private ARTrackedImageManager trackedImageManager;

    private void Awake()
    {
        trackedImageManager = FindObjectOfType<ARTrackedImageManager>();
    }

    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += ImageChanged;
    }

    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= ImageChanged;
    }

    private void ImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            AddTrackable(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            RemoveTrackable(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            UpdateHistory(trackedImage);
            trackedImage.gameObject.transform.position = CalculatePosition(trackedImage);
            trackedImage.gameObject.transform.eulerAngles = CalculateRotation(trackedImage);
        }
    }

    private void AddTrackable(ARTrackedImage trackedImage)
    {
        history.Add(trackedImage, new TrackedImageData()
        {
            lastPosition = trackedImage.transform.position,
            lastRotation = trackedImage.transform.eulerAngles,
            velocity = Vector3.zero,
            rotation = Vector3.zero
        });
    }

    private void RemoveTrackable(ARTrackedImage trackedImage)
    {
        history.Remove(trackedImage);
    }

    private void UpdateHistory(ARTrackedImage trackedImage)
    {
        history[trackedImage].lastPosition = trackedImage.gameObject.transform.position;
        history[trackedImage].lastRotation = trackedImage.gameObject.transform.eulerAngles;
    }

    private TrackedImageData GetTrackedImage(ARTrackedImage trackedImage)
    {
        return history[trackedImage];
    }

    private Vector3 CalculatePosition(ARTrackedImage trackedImage)
    {
        TrackedImageData trackedImageData = GetTrackedImage(trackedImage);
        return Vector3.SmoothDamp(trackedImageData.lastPosition, trackedImage.transform.position, ref trackedImageData.velocity, 0.1f, 100);
    }

    private Vector3 CalculateRotation(ARTrackedImage trackedImage)
    {
        TrackedImageData trackedImageData = GetTrackedImage(trackedImage);
        return Vector3.SmoothDamp(trackedImageData.lastRotation, trackedImage.transform.eulerAngles, ref trackedImageData.rotation, 1f, 100);
    }

    private class TrackedImageData
    {
        public Vector3 lastPosition;
        public Vector3 lastRotation;
        public Vector3 velocity;
        public Vector3 rotation;
    }
}
