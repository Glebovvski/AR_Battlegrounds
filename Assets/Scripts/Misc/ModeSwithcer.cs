using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeSwithcer : MonoBehaviour
{
    public GameObject dropObjects;
    public GameObject moveObjects;

    private int appMode;
    // Start is called before the first frame update
    void Start()
    {
        appMode = 0;
        dropObjects.SetActive(appMode == 0);
        moveObjects.SetActive(appMode == 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonToggle(Text description)
    {
        appMode = (appMode + 1) % 2;
        description.text = (appMode == 0) ? "Drop Mode" : "Push Mode";

        dropObjects.SetActive(appMode == 0);
        moveObjects.SetActive(appMode == 1);
    }
}
