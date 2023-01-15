using System.Collections;
using System.Collections.Generic;
using Defendable;
using Enemies;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private GameGrid grid;
    [SerializeField] private LayerMask GridLayer;
    [SerializeField] private LayerMask DefenseLayer;
    [SerializeField] private LayerMask EnemyLayer;

    private Defense SelectedDefense { get; set; }

    private void Update()
    {
        var cell = GetObjectOnScene<GridCell>(GridLayer);
        if (!cell || !cell.IsSelected) return;

        var defense = GetObjectOnScene<Defense>(DefenseLayer);
        if (!defense || !defense.IsActiveDefense) return;
        SelectedDefense = defense;

        var enemy = GetObjectOnScene<Enemy>(EnemyLayer);
        if(!enemy || !enemy.IsAlive || !SelectedDefense) return;
        // SelectedDefense.SetAsAttackTarget(enemy);

        if (Input.GetMouseButtonDown(0))
        {
            SpawnOnCell(cell);
        }
    }

    private void SpawnOnCell(GridCell cell)
    {
        grid.SpawnDefence(cell);
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
