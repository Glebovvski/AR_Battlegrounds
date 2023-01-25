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

    private void Start()
    {
        Grid.transform.position = Camera.main.transform.position - Vector3.back*1000;
    }

    private void Update()
    {
        if (Input.touchCount == 0) return;

        var touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
            Ray raycast = Camera.main.ScreenPointToRay(touch.position);
            if (Physics.Raycast(raycast, out RaycastHit raycastHit, float.MaxValue))
            {
                if (raycastHit.collider.TryGetComponent<ARPlane>(out var plane))
                {
                    this.transform.localScale = new Vector3(plane.size.x, 1, plane.size.y);
                    Grid.CreateGrid();
                    origin.MakeContentAppearAt(this.transform, plane.center, Quaternion.identity);
                    planeManager.requestedDetectionMode = PlaneDetectionMode.None;
                    Debug.LogError("GRID CREATED");
                }
                else
                DebugView.Instance.SetText("NO AR PLANE");
            }
            else
            DebugView.Instance.SetText("NO RAYCAST TARGET");
        }
    }
}
