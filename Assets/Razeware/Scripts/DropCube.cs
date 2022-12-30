using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class DropCube : MonoBehaviour
{
    [SerializeField] GameObject presetCube;
    [SerializeField] ARPlaneManager planeManager;
    [SerializeField] Text error;
    [SerializeField] CharacterMain prefabToDrop;
    private Camera camera;
    private Vector3 position;
    List<CharacterMain> cubes;
    [SerializeField] private GameObject targetPrefab;
    [SerializeField] private GameObject target;
    [SerializeField] private bool isDestinationClick;
    [SerializeField] private bool detectPlanes;
    [SerializeField] private bool isSpawning;
    [SerializeField] private Transform castle;
    [SerializeField] private bool firstClick = true;

    private int layerMask;

    [SerializeField] private ARSessionOrigin origin;
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        presetCube.SetActive(false);
        target = Instantiate(targetPrefab);
        detectPlanes = true;
        cubes = new List<CharacterMain>();
        isDestinationClick = false;
        isSpawning = true;
        //layerMask = LayerMask.NameToLayer("ARGameObject");
        planeManager.planesChanged += PlaneManager_planesChanged;
    }

    private void PlaneManager_planesChanged(ARPlanesChangedEventArgs obj)
    {
        if (detectPlanes)
        {
            foreach (var plane in obj.updated)
            {
                var navMesh = plane.GetComponent<NavMeshSurface>();
                navMesh.UpdateNavMesh(navMesh.navMeshData);
            }
            foreach (var plane in obj.added)
            {
                var navMesh = plane.GetComponent<NavMeshSurface>();
                navMesh.BuildNavMesh();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isSpawning)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Moved && !IsPointerOverGameObject())// || Input.GetTouch(0).phase == TouchPhase.Began)
            {
                if (!isDestinationClick)
                {
                    Ray ray = camera.ScreenPointToRay(Input.touches[0].position);
                    RaycastHit rayHit;
                    if (Physics.Raycast(ray, out rayHit, float.MaxValue))//, layerMask))
                    {
                        presetCube.SetActive(true);
                        presetCube.transform.position = rayHit.point;
                    }
                }
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Began && !IsPointerOverGameObject())
            {
                if (isDestinationClick)
                {
                    Ray ray = camera.ScreenPointToRay(Input.touches[0].position);
                    RaycastHit rayHit;
                    if (Physics.Raycast(ray, out rayHit, float.MaxValue)) //, layerMask))
                    {
                        position = rayHit.point;
                        target.transform.position = rayHit.point;

                        MoveTo(position);
                    }
                }
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended && !IsPointerOverGameObject())
            {
                if (!isDestinationClick)
                {
                    CharacterMain cube = Instantiate(prefabToDrop, presetCube.transform.position, Quaternion.identity);
                    cube.Construct(origin, error);
                    //var rigidbody = cube.AddComponent<Rigidbody>();
                    //rigidbody.mass = 0.01f;
                    //rigidbody.freezeRotation
                    cubes.Add(cube);
                    presetCube.SetActive(false);
                }
            }
        }

    }

    public void RemoveAllCubes()
    {
        foreach (var cube in cubes)
        {
            Destroy(cube);
        }
        cubes.Clear();
    }

    public void ChangeTouchType()
    {
        presetCube.SetActive(false);
        isDestinationClick = !isDestinationClick;
        if (isDestinationClick) error.text = "AI ON";
        else error.text = "AI OFF";
    }

    public void MoveTo(Vector3 position)
    {
        // var surface = GameObject.FindObjectOfType<NavMeshSurface>();
        //surface.buildHeightMesh = true;
        //surface.UpdateNavMesh(surface.navMeshData);
        // surface.BuildNavMesh();
        foreach (var cube in cubes)
        {
            //agent.Warp(new Vector3(cube.transform.position.x, surface.transform.position.y, cube.transform.position.z));
            //if (!agent.isOnNavMesh) agent.Warp(position);

            //agent.destination = position;
            cube.SetDestination(position);

            //if (position.y != agent.transform.position.y)
            //{
            //    agent.transform.position = new Vector3(agent.transform.position.x, position.y*10, agent.transform.position.z);
            //    error.text = "POSITION: " + position.ToString();
            //    error.text = "AGENT: " + agent.transform.position.ToString();
            //}


            // error.text = "PATH STATUS: "+agent.pathStatus.ToString()+"\r\n";
            // error.text += agent.isOnNavMesh.ToString();

            //error.text += warped.ToString();//surface.isActiveAndEnabled.ToString();//.transform.localScale.ToString();
        }
    }

    public void TogglePlaneDetection()
    {
        ARPlane plane = FindObjectOfType<ARPlane>();

        if (!plane) error.text = "NO PLANE DETECTED";

        //presetCube.SetActive(false);
        detectPlanes = !detectPlanes;
        if (detectPlanes)
        {
            planeManager.detectionMode = PlaneDetectionMode.Horizontal;
        }
        else planeManager.detectionMode = PlaneDetectionMode.None;

        var cube1 = Instantiate(castle, plane.transform);
        origin.MakeContentAppearAt(cube1.transform, plane.transform.position, Quaternion.identity);
    }

    public void ToggleSpawn()
    {
        isSpawning = !isSpawning;
    }

    private bool IsPointerOverGameObject()
    {
        PointerEventData eventDataCurrentPos = new PointerEventData(EventSystem.current);
        eventDataCurrentPos.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPos, results);
        return results.Count > 0;
    }
}
