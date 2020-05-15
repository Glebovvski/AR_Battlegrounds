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
    public GameObject presetCube;
    public ARPlaneManager planeManager;
    public Text error;
    public GameObject prefabToDrop;
    private new Camera camera;
    //public ARSessionOrigin aRSession;
    private Vector3 position;
    List<GameObject> cubes;
    public GameObject targetPrefab;
    private GameObject target;
    private bool isDestinationClick;
    private bool detectPlanes;
    private bool isSpawning;
    public GameObject cube;
    private bool firstClick = true;

    private int layerMask;

    public ARSessionOrigin origin;
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        presetCube.SetActive(false);
        target = Instantiate(targetPrefab);
        detectPlanes = true;
        cubes = new List<GameObject>();
        isDestinationClick = false;
        isSpawning = true;
        //layerMask = LayerMask.NameToLayer("ARGameObject");
        //planeManager.planesChanged += PlaneManager_planesChanged;
    }

    //private void PlaneManager_planesChanged(ARPlanesChangedEventArgs obj)
    //{
    //    foreach (var plane in obj.updated)
    //    {
    //        if (plane)
    //        {
    //            var surface = plane.GetComponent<NavMeshSurface>();
    //            surface.BuildNavMesh();//UpdateNavMesh(surface.navMeshData);
    //        }
    //    }
    //}

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
                        GameObject cube = Instantiate(prefabToDrop, presetCube.transform.position, Quaternion.identity);
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
        var surface = GameObject.FindObjectOfType<NavMeshSurface>();
        surface.buildHeightMesh = true;
        surface.BuildNavMesh();
        foreach (var cube in cubes)
        {
            NavMeshAgent agent = cube.GetComponent<NavMeshAgent>();
            //agent.Warp(new Vector3(cube.transform.position.x, surface.transform.position.y, cube.transform.position.z));
            agent.destination = position;

            //if (!agent.isOnNavMesh) agent.Warp(position);
           
            error.text = "PATH STATUS: "+agent.pathStatus.ToString()+"\r\n";
            error.text += agent.isOnNavMesh.ToString();
            //error.text += warped.ToString();//surface.isActiveAndEnabled.ToString();//.transform.localScale.ToString();
        }
    }

    public void TogglePlaneDetection()
    {
        ARPlane plane = FindObjectOfType<ARPlane>();

        if (!plane) error.text = "NO PLANE DETECTED";

        presetCube.SetActive(false);
        detectPlanes = !detectPlanes;
        if (detectPlanes)
        {
            planeManager.detectionMode = PlaneDetectionMode.Horizontal;
        }
        else planeManager.detectionMode = PlaneDetectionMode.None;

        var cube1 = Instantiate(cube, plane.transform);
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
