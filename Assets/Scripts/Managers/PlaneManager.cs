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

    private bool gridCreated = false;

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
            Ray raycast = Camera.main.ScreenPointToRay(touch.position);
            if (Physics.Raycast(raycast, out RaycastHit raycastHit, float.MaxValue))
            {
                if (raycastHit.collider.TryGetComponent<ARPlane>(out var plane))
                {
                    // this.transform.localScale = new Vector3(this.transform.localScale.x / plane.size.x, 1, this.transform.localScale.z / plane.size.y);
                    origin.MakeContentAppearAt(this.transform, plane.center, Quaternion.identity);
                    Grid.CreateGrid();
                    planeManager.requestedDetectionMode = PlaneDetectionMode.None;
                    gridCreated = true;
                    DebugView.Instance.SetText("GRID CREATED");
                }
                else
                    DebugView.Instance.SetText("NO AR PLANE");
            }
            else
                DebugView.Instance.SetText("NO RAYCAST TARGET");
        }
    }
}
