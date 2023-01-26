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
    [SerializeField] private Transform missileCollider;

    private bool gridCreated = false;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    public event Action OnGridSet;

    private void Start()
    {
        Grid.OnGridCreated += SetUpGrid;
    }

    private void SetUpGrid()
    {
        var scale = Grid.transform.localScale;
        Grid.transform.localScale = new Vector3(scale.x / this.transform.localScale.x, scale.y / this.transform.localScale.y, scale.z / this.transform.localScale.z);
        // this.transform.localScale = new Vector3(Grid.Width/10, 1, Grid.Length/10);
        // missileCollider.localScale = this.transform.localScale;
        // this.transform.localScale = new Vector3(Grid.Width, 1, Grid.Length);
        // Grid.transform.SetParent(this.transform);
        OnGridSet?.Invoke();
    }

    private void Update()
    {
        if (gridCreated) return;
#if PLATFORM_IOS
        if (Input.touchCount == 0) return;
        if (planeManager.trackables.count == 0) return;

        var touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
            // Ray ray = Camera.main.ScreenPointToRay(touch.position);
            // RaycastHit rayHit;
            // if (Physics.Raycast(ray, out rayHit, float.MaxValue))//, layerMask))
            // {
            //     // if(rayHit.)
            //     // var plane = planeManager.GetPlane(hits[0].trackableId);
            //     // if (!plane) return;
            //     // var hitPose = hits[0].pose;
            //     planeManager.requestedDetectionMode = PlaneDetectionMode.None;
            //     this.transform.position = rayHit.point;

            //     Grid.CreateGrid();
            //     DebugView.Instance.SetText("GRID CREATED");
            //     gridCreated = true;
            // }
            if (arRaycastManager.Raycast(touch.position, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes))
            {
                var plane = planeManager.GetPlane(hits[0].trackableId);
                if (!plane)
                {
                    DebugView.Instance.SetText("NO PLANE FOUND");
                    return;
                }
                var hitPose = hits[0].pose;
                planeManager.requestedDetectionMode = PlaneDetectionMode.None;
                this.transform.position = hitPose.position;// plane.center;//.position;
                this.transform.rotation = hitPose.rotation;

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
#elif UNITY_EDITOR
        Grid.CreateGrid();
        gridCreated = true;

#endif
    }
}

