using System;
using Defendable;
using Enemies;
using UnityEngine;
using Zenject;

public class InputManager : MonoBehaviour
{
    private GameTimeModel GameTimeModel { get; set; }
    private GameGrid Grid { get; set; }
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

    [Inject]
    private void Construct(GameGrid gameGrid)
    {
        // GameTimeModel = gameTimeModel;
        Grid = gameGrid;
    }

    private void Start()
    {
        GridLayer = LayerMask.GetMask("Grid");
        DefenseLayer = LayerMask.GetMask("Defense");
        EnemyLayer = LayerMask.GetMask("Enemy");
    }

    private void Update()
    {
        // if (!GameTimeModel.IsPaused) return;

        var cell = GetObjectOnScene<GridCell>(GridLayer);
        if (Input.GetMouseButtonDown(0))
        {
            if (!TrySpawnOnCell(cell))
            {
                var defense = GetObjectOnScene<ActiveDefense>(DefenseLayer);
                if (!defense || !defense.IsActiveDefense) return;
                SelectedDefense = defense;

                if (SelectedDefense)
                {
                    var enemy = GetObjectOnScene<Enemy>(EnemyLayer);
                    if (!enemy || !enemy.IsAlive || !SelectedDefense || !SelectedDefense.IsEnemyInRange(enemy)) return;
                    SelectedDefense.SetAttackTarget(enemy);
                    OnEnemyClick?.Invoke();
                }
            }
        }
        // if (!cell || !cell.IsSelected) return;


    }

    private bool TrySpawnOnCell(GridCell cell)
    {
        if (!cell || !cell.IsSelected)
            return false;
        Grid.SpawnDefence(cell);
        return true;
    }

    private T GetObjectOnScene<T>(LayerMask layer) where T : MonoBehaviour
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var raycastHit, layer))
        {
            return raycastHit.transform.GetComponent<T>();
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
