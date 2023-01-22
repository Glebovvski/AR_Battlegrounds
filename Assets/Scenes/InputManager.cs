using System;
using Defendable;
using Enemies;
using UnityEngine;
using Zenject;

public class InputManager : MonoBehaviour
{
    private DefensesModel DefensesModel { get; set; }
    private GameGrid Grid { get; set; }
    private LayerMask GridLayer;
    private LayerMask DefenseLayer;
    private LayerMask EnemyLayer;

    public event Action OnActiveDefenseClick;
    public event Action OnEnemyClick;

    private ActiveDefense _selectedDefense;
    public ActiveDefense SelectedDefense
    {
        get => _selectedDefense;
        private set
        {
            if (_selectedDefense) _selectedDefense.SelectDefense(false); //set outline off for previous defense
            _selectedDefense = value;
            if (_selectedDefense)
            {
                _selectedDefense.SelectDefense(true);
                OnActiveDefenseClick?.Invoke();
            }
        }
    }

    [Inject]
    private void Construct(GameGrid gameGrid, DefensesModel defensesModel)
    {
        DefensesModel = defensesModel;
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

        if (Input.GetMouseButtonDown(0))
        {
            var cell = GetObjectOnScene<GridCell>(GridLayer);
            if (!TrySpawnOnCell(cell))
            {
                if (DefensesModel.InDefenseSelectionMode) return;

                var defense = GetObjectOnScene<ActiveDefense>(DefenseLayer);
                if ((!defense || !defense.IsActiveDefense) && !SelectedDefense) return;
                if (SelectedDefense != defense && defense)
                {
                    SelectedDefense = defense;
                    return;
                }

                if (SelectedDefense)
                {
                    var enemy = GetObjectOnScene<Enemy>(EnemyLayer);
                    if (!enemy || !enemy.IsAlive || !SelectedDefense || !SelectedDefense.IsEnemyInRange(enemy)) return;
                    SelectedDefense.SetAttackTarget(enemy);
                    OnEnemyClick?.Invoke();
                    SelectedDefense = null;
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
        if (Physics.Raycast(ray, out var raycastHit, 1000f, layer))
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
