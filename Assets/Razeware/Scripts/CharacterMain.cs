using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class CharacterMain : MonoBehaviour
{
    public Transform sphere;
    public Text error;

    public Material free;
    public Material drag;

    private ARSessionOrigin origin;

    private bool climbing;

    Vector3 distanceSphere;
    // Start is called before the first frame update
    void Start()
    {
        error = FindObjectOfType<Text>();
        origin = FindObjectOfType<ARSessionOrigin>();
        climbing = false;
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
        //if (!climbing)
        //{
            sphere.transform.position = new Vector3(pos_move.x /*- distance.x*/, sphere.transform.position.y, pos_move.z);
            transform.position = new Vector3(pos_move.x /*- distance.x*/, transform.position.y, pos_move.z);
        //}
        //else
        //{
        //    sphere.transform.position = pos_move;// new Vector3(pos_move.x /*- distance.x*/, sphere.transform.position.y, pos_move.z);
        //    transform.position = pos_move;// new Vector3(pos_move.x /*- distance.x*/, pos_move.y - sphere.transform.position.y, pos_move.z);
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "wall")
        {
            climbing = true;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            float height = (other.transform.localScale.y + transform.localScale.y);// + transform.localScale.y;
            error.text = "SUPPOESED HEIGHT: " + other.transform.localScale.y.ToString() + "\nACTUAL HEIGHT: " + height.ToString();
            transform.position = new Vector3(transform.position.x, height, transform.position.z);
            Debug.Log("ENTER TRIGGER");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "wall")
        {
            climbing = false;
            Debug.Log("EXIT TRIGGER");
        }
    }


    private void OnMouseUp()
    {
        sphere.GetComponent<MeshRenderer>().material = free;
    }
}
