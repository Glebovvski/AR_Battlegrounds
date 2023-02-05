using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Zenject;

public class PlaneManager : MonoBehaviour
{
    private MenuViewModel MenuViewModel { get; set; }

    [SerializeField] private ARPlaneManager planeManager;
    [SerializeField] private ARSessionOrigin origin;
    [SerializeField] private GameGrid Grid;
    [SerializeField] private ARRaycastManager arRaycastManager;
    [SerializeField] private Transform missileCollider;

    private NavMeshSurface surface;

    public bool GridCreated { get; private set; } = false;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private float planeFactor = 2.5f;

    public event Action OnGridSet;

    public Vector3 Scale => transform.localScale;

    [Inject]
    private void Construct(MenuViewModel menuViewModel)
    {
        MenuViewModel = menuViewModel;
    }

    private void Start()
    {
        Grid.OnGridCreated += SetUpGrid;
        surface = GetComponent<NavMeshSurface>();
    }

    private void SetUpGrid()
    {
        AttachChild(Grid.transform);
        OnGridSet?.Invoke();
        UpdateNavMesh();
    }

    public void UpdateNavMesh() => surface.BuildNavMesh();

    public void AttachChild(Transform child)
    {
        if (child.parent == this.transform) return;

        var scale = child.localScale;
        child.localScale = new Vector3(scale.x / (this.transform.localScale.x * planeFactor), scale.y / (this.transform.localScale.y * planeFactor), scale.z / (this.transform.localScale.z * planeFactor));
        child.SetParent(this.transform);
    }

    private void Update()
    {
        if (GridCreated) return;
        if (MenuViewModel.IsMenuOpen) return;

        // #if PLATFORM_IOS || PLATFORM_ANDROID
        //         if (Input.touchCount == 0) return;
        //         if (planeManager.trackables.count == 0) return;

        //         var touch = Input.GetTouch(0);
        //         if (touch.phase == TouchPhase.Began)
        //         {
        //             Ray raycast = Camera.main.ScreenPointToRay(touch.position);
        //             if (Physics.Raycast(raycast, out RaycastHit raycastHit, float.MaxValue))
        //             {
        //                 if (raycastHit.collider.TryGetComponent<ARPlane>(out var plane))
        //                 {
        //                     origin.MakeContentAppearAt(this.transform, plane.center, Quaternion.identity);
        //                     Grid.CreateGrid();
        //                     GridCreated = true;
        //                     planeManager.requestedDetectionMode = PlaneDetectionMode.None;
        //                     RemovePlanes();
        //                     DebugView.Instance.SetText("GRID CREATED");
        //                 }
        //                 else
        //                     DebugView.Instance.SetText("NO AR PLANE");
        //             }
        //             else
        //                 DebugView.Instance.SetText("NO RAYCAST TARGET");
        //         }
        // #elif UNITY_EDITOR
        Grid.CreateGrid();
        GridCreated = true;

        // #endif
    }

    private void RemovePlanes()
    {
        foreach (var plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }
    }
}

