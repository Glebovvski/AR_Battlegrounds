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

    private void Update()
    {
        var touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
            Ray raycast = Camera.main.ScreenPointToRay(touch.position);
            if (Physics.Raycast(raycast, out RaycastHit raycastHit))
            {
                if (raycastHit.collider.TryGetComponent<ARPlane>(out var plane))
                {
                    origin.MakeContentAppearAt(this.transform, plane.center, Quaternion.identity);
                    planeManager.requestedDetectionMode = PlaneDetectionMode.None;
                }
            }
        }
    }
}
