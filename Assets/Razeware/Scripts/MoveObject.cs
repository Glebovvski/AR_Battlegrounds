using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public GameObject wreckerGo;
    public new Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActiveAndEnabled) return;

        if(Input.GetTouch(0).phase== TouchPhase.Moved)
        {
            Ray ray = camera.ScreenPointToRay(Input.touches[0].position);

            RaycastHit rayHit;
            if (Physics.Raycast(ray, out rayHit, float.MaxValue))
            {
                if (wreckerGo.activeSelf)
                {
                    wreckerGo.transform.position = Vector3.MoveTowards(wreckerGo.transform.position, rayHit.point, 0.1f);
                }
                else
                {
                    wreckerGo.transform.position = rayHit.point;
                    wreckerGo.SetActive(true);
                }
            }
            else wreckerGo.SetActive(false);
        }
    }
}
