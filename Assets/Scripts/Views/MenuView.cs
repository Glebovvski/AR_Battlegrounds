using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MenuView : MonoBehaviour
{
    private MenuViewModel MenuViewModel { get; set; }

    [SerializeField] private GameObject menuPanel;

    [SerializeField] private Button startBtn;
    [SerializeField] private Button buyBtn;
    [SerializeField] private Button donateBtn;

    [Inject]
    private void Construct(MenuViewModel menuViewModel)
    {
        MenuViewModel = menuViewModel;
    }

    private void Start()
    {
        MenuViewModel.OnOpen += Show;
    }

    public void Close()
    {
        menuPanel.SetActive(false);
        MenuViewModel.Close();
    }

    public void Show()
    {
        menuPanel.SetActive(true);
    }

    private void OnDestroy()
    {
        MenuViewModel.OnOpen -= Show;
    }
}
