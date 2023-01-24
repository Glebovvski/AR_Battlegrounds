using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlaneManager : MonoBehaviour
{
    [SerializeField] private ARPlaneManager planeManager;
    [SerializeField] private ARSessionOrigin origin;

    private void Start()
    {
        planeManager.planesChanged += PlanesChanged;
    }

    private void PlanesChanged(ARPlanesChangedEventArgs obj)
    {
        if(obj.added.Count == 0) return;

        var plane = obj.added[0];
        origin.MakeContentAppearAt(this.transform, Vector3.zero, Quaternion.identity);
    }
}
