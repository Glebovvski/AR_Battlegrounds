using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Image healthBar;
    private Transform target;

    private void Start()
    {
        canvas.worldCamera = Camera.main;
    }

    private void Update()
    {
        this.transform.LookAt(Camera.main.transform);
    }

    public void UpdateHealth(float health)
    {
        healthBar.fillAmount = health;
    }
}
