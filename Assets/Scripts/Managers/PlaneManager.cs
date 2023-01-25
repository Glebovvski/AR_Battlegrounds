using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaneManager : MonoBehaviour
{
    [SerializeField] private ARPlaneManager planeManager;
    [SerializeField] private ARSessionOrigin origin;
    [SerializeField] private GameGrid Grid;
    [SerializeField] private ARRaycastManager arRaycastManager;

    private bool gridCreated = false;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private void Start()
    {
        // Grid.transform.position = Camera.main.transform.position - Vector3.back*1000;
    }

    private void Update()
    {
        if (gridCreated) return;
        if (Input.touchCount == 0) return;
        if (planeManager.trackables.count == 0) return;

        var touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
            if (arRaycastManager.Raycast(touch.position, hits, UnityEngine.XR.ARSubsystems.TrackableType.AllTypes))
            {
                var plane = planeManager.GetPlane(hits[0].trackableId);
                if(!plane) return;
                var hitPose = hits[0].pose;
                planeManager.requestedDetectionMode = PlaneDetectionMode.None;
                this.transform.position = plane.center;//.position;
                // this.transform.rotation = hitPose.rotation;
                
                Grid.CreateGrid();
                DebugView.Instance.SetText("GRID CREATED");
                gridCreated = true;
            }
            // Ray raycast = Camera.main.ScreenPointToRay(touch.position);
            // if (Physics.Raycast(raycast, out RaycastHit raycastHit, float.MaxValue))
            // {
            //     if (raycastHit.collider.TryGetComponent<ARPlane>(out var plane))
            //     {
            //         // this.transform.localScale = new Vector3(this.transform.localScale.x / plane.size.x, 1, this.transform.localScale.z / plane.size.y);
            //         // this.transform.SetParent(plane.transform);
            //         origin.MakeContentAppearAt(this.transform, plane.center, Quaternion.identity);
            //         DebugView.Instance.SetText("GRID CREATED");
            //     }
            //     else
            //         DebugView.Instance.SetText("NO AR PLANE");
            // }
            else
                DebugView.Instance.SetText("NO RAYCAST TARGET");
        }
    }
}
