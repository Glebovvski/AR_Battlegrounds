using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuView : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;

    [SerializeField] private Button startBtn;
    [SerializeField] private Button buyBtn;
    [SerializeField] private Button donateBtn;

    private void Start()
    {

    }

    public void Show()
    {
        menuPanel.SetActive(true);
    }
}
