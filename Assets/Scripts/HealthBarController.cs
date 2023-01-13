using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] private bool isAlwaysActive = false;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Image healthBar;
    private Transform target;

    private void Start()
    {
        if (!isAlwaysActive) healthBar.gameObject.SetActive(false);
        canvas.worldCamera = Camera.main;
    }

    private void Update()
    {
        this.transform.LookAt(Camera.main.transform);
    }

    public void UpdateHealth(float health)
    {
        if (!isAlwaysActive) healthBar.gameObject.SetActive(true);
        healthBar.fillAmount = health;
    }
}
