using System;
using Defendable;
using Enemies;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private GameGrid Grid => AIManager.Instance.Grid;
    private LayerMask GridLayer;
    private LayerMask DefenseLayer;
    private LayerMask EnemyLayer;

    public event Action OnActiveDefenseClick;
    public event Action OnEnemyClick;

    private ActiveDefense SelectedDefense
    {
        get => SelectedDefense;
        set
        {
            SelectedDefense = value;
            OnActiveDefenseClick?.Invoke();
        }
    }

    private void Start()
    {
        GridLayer = LayerMask.GetMask("Grid");
        DefenseLayer = LayerMask.GetMask("Defense");
        EnemyLayer = LayerMask.GetMask("Enemy");
    }

    private void Update()
    {
        var cell = GetObjectOnScene<GridCell>(GridLayer);
        if (!cell || !cell.IsSelected) return;

        if (Input.GetMouseButtonDown(0))
        {
            SpawnOnCell(cell);
        }


        var defense = GetObjectOnScene<ActiveDefense>(DefenseLayer);
        if (!defense || !defense.IsActiveDefense) return;
        SelectedDefense = defense;

        var enemy = GetObjectOnScene<Enemy>(EnemyLayer);
        if (!enemy || !enemy.IsAlive || !SelectedDefense || !SelectedDefense.IsEnemyInRange(enemy)) return;
        SelectedDefense.SetAttackTarget(enemy);
        OnEnemyClick?.Invoke();

    }

    private void SpawnOnCell(GridCell cell)
    {
        Grid.SpawnDefence(cell);
    }

    private T GetObjectOnScene<T>(LayerMask layer) where T : MonoBehaviour
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var raycastHitGrid, layer))
        {
            return raycastHitGrid.transform.GetComponent<T>();
        }
        return null;
    }

    // [SerializeField] private CharacterMain player;
    // [SerializeField] private Transform target;

    // public void Update()
    // {
    //     if (!Input.GetMouseButtonDown(0))
    //         return;

    //     var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

    //     RaycastHit hit = new RaycastHit();

    //     if (Physics.Raycast(ray, out hit))
    //     {
    //         target.position = hit.point;
    //         player.SetDestination(target.position);
    //     }
    // }

}
