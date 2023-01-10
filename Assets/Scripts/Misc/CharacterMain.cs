using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class CharacterMain : MonoBehaviour
{
    [SerializeField] Transform sphere;
    [SerializeField] Text error;

    [SerializeField] Material free;
    [SerializeField] Material drag;

    [SerializeField] NavMeshAgent agent;
    private ARSessionOrigin origin;


    Vector3 distanceSphere;

    public void Construct(ARSessionOrigin origin, Text error)
    {
        this.origin = origin;
        this.error = error;
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void SetDestination(Vector3 position)
    {
        agent.destination = position;
    }

    // Update is called once per frame
    void Update()
    {
        //button.transform.LookAt(Camera.main.transform);
    }

    void OnMouseDown()
    { 
        //distanceSphere = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,0)) - sphere.transform.position;
        sphere.GetComponent<MeshRenderer>().material = drag;
    }

    void OnMouseDrag()
    {
        Vector3 distance_to_screen = Camera.main.WorldToScreenPoint(sphere.transform.position);
        Vector3 pos_move = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance_to_screen.z));
        sphere.transform.position = new Vector3(pos_move.x /*- distance.x*/, sphere.transform.position.y, pos_move.z);
        transform.position = new Vector3(pos_move.x /*- distance.x*/, transform.position.y, pos_move.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.tag == "wall")
        //{
        //    GetComponent<Rigidbody>().velocity = Vector3.zero;
        //    float height = (other.transform.localScale.y + transform.localScale.y);// + transform.localScale.y;
        //    error.text = "SUPPOESED HEIGHT: " + other.transform.localScale.y.ToString() + "\nACTUAL HEIGHT: " + height.ToString();
        //    transform.position = new Vector3(transform.position.x, height, transform.position.z);
        //    Debug.Log("ENTER TRIGGER");
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "wall")
        {
            Debug.Log("EXIT TRIGGER");
        }
    }


    private void OnMouseUp()
    {
        sphere.GetComponent<MeshRenderer>().material = free;
    }
}