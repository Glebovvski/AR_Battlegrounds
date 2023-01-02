using System;
using System.Collections.Generic;
using System.Linq;
using Defendable;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    [SerializeField] private DefencesViewModel defencesViewModel;
    [SerializeField] private DefencesModel defencesModel;

    [SerializeField] private GridCell gridCellPrefab;
    [SerializeField] private CastleDefence castlePrefab;
    private int width = 15;
    private int length = 15;
    private float gridSpaceSize = 1f;

    public List<GridCell> GridList = new List<GridCell>();

    private GridCell[,] grid;

    private List<GridCell> centreCells;

    public PoolObjectType SelectedDefence { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        defencesViewModel.OnDefenseSelected += SelectDefence;
        defencesViewModel.OnDefenseDeselected += DeselectDefense;

        CreateGrid();
    }

    public Vector2Int GetGridPositionFromWorld(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt(worldPosition.x / gridSpaceSize);
        int y = Mathf.FloorToInt(worldPosition.z / gridSpaceSize);

        x = Mathf.Clamp(x, 0, width);
        y = Mathf.Clamp(y, 0, length);

        return new Vector2Int(x, y);
    }

    public Vector3 GetWorldPositionFromGrid(GridCell cell)
    {
        float x = cell.Pos.x * gridSpaceSize;
        float y = cell.Pos.y * gridSpaceSize;

        return new Vector3(x, cell.Height, y);
    }

    public Vector3 GetWorldPositionFromGrid(Vector2Int position, int height)
    {
        float x = position.x * gridSpaceSize;
        float y = position.y * gridSpaceSize;

        return new Vector3(x, height, y);
    }

    public Vector3 GetWorldPositionFromGrid(Vector2 position, int height)
    {
        float x = position.x * gridSpaceSize;
        float y = position.y * gridSpaceSize;

        return new Vector3(x, height, y);
    }

    public void SelectDefence(PoolObjectType type)
    {
        SelectedDefence = type;
        var defenseInfo = PoolManager.Instance.GetFromPool<Defence>(type).SO;
        DeselectAllCells();
        if (defenseInfo.Size == Vector2.one)
        {
            var match = GridList.FindAll(defenseInfo.GetCondition()).FindAll(ConditionsData.IsEmptyCell);
            match.ForEach(x => x.SetSelected());
        }
    }

    public void DeselectDefense()
    {
        DeselectAllCells();
        SelectedDefence = PoolObjectType.None;
    }

    public void SpawnDefence(GridCell cell)
    {
        if (SelectedDefence == PoolObjectType.None) return;
        
        var defence = PoolManager.Instance.GetFromPool<Defence>(SelectedDefence);
        defence.transform.position = GetWorldPositionFromGrid(cell);
        cell.SetDefence(defence);
    }


    private (bool, Quaternion) IsCornerCell(GridCell cell)
    {
        if (cell.Pos.x == width - 1 && cell.Pos.y == width - 1) return (true, Quaternion.Euler(0, 45, 0));
        if (cell.Pos.x == width - 1 && cell.Pos.y == 0) return (true, Quaternion.Euler(0, 135, 0));
        if (cell.Pos.x == 0 && cell.Pos.y == width - 1) return (true, Quaternion.Euler(0, -45, 0));
        if (cell.Pos.x == 0 && cell.Pos.y == 0) return (true, Quaternion.Euler(0, 225, 0));
        return (false, Quaternion.identity);
    }

    private void CreateGrid()
    {
        grid = new GridCell[width, length];
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < length; z++)
            {
                grid[x, z] = Instantiate(gridCellPrefab, new Vector3(x * gridSpaceSize, 0, z * gridSpaceSize), Quaternion.Euler(90, 0, 0));
                grid[x, z].Init(x, z);
                grid[x, z].gameObject.name = string.Format("Cell {0}:{1}", x, z);
                grid[x, z].transform.parent = transform;
                GridList.Add(grid[x, z]);
            }
        }
        TryChangeHeight();
        SpawnCastleAtCentre();
    }

    private void SpawnCastleAtCentre()
    {
        Vector2 centre = centreCells.Aggregate(Vector2.zero, (acc, v) => acc + v.Pos) / centreCells.Count;
        var castle = Instantiate(castlePrefab, GetWorldPositionFromGrid(centre, centreCells[0].Height), Quaternion.identity);
        centreCells.ForEach(x => x.SetDefence(castle));
    }

    private void TryChangeHeight(GridCell cell)
    {
        if (cell.Pos.x == 0 || cell.Pos.y == 0 || cell.Pos.x == width - 1 || cell.Pos.y == length - 1)
            return;

        cell.SetHeight(UnityEngine.Random.Range(1, 3));
    }

    private void TryChangeHeight()
    {
        int centre = GridList.Count / 2;

        var gridCell1 = GridList[centre];
        var gridCell2 = grid[gridCell1.Pos.x, gridCell1.Pos.y - 1];
        var gridCell3 = grid[gridCell1.Pos.x + 1, gridCell1.Pos.y];
        var gridCell4 = grid[gridCell1.Pos.x + 1, gridCell1.Pos.y - 1];
        centreCells = new List<GridCell>()
        {
            gridCell1, gridCell2, gridCell3, gridCell4
        };

        foreach (var cell in GridList)
        {
            if (IsMustHaveGroundHeight(cell))
                continue;
            if (IsAnyDiagonalCellUp(cell))
                continue;
            cell.SetHeight(UnityEngine.Random.Range(1, 3));
        }

    }

    private bool IsMustHaveGroundHeight(GridCell cell)
    {
        return cell.Pos.x == 0 || cell.Pos.y == 0 || cell.Pos.x == width - 1 || cell.Pos.y == length - 1 || centreCells.Contains(cell);
    }

    private bool IsAnyDiagonalCellUp(GridCell cell)
    {
        return grid[cell.Pos.x - 1, cell.Pos.y - 1].IsUpper || grid[cell.Pos.x - 1, cell.Pos.y + 1].IsUpper
                || grid[cell.Pos.x + 1, cell.Pos.y + 1].IsUpper || grid[cell.Pos.x + 1, cell.Pos.y - 1].IsUpper;
    }

    private void DeselectAllCells() => GridList.ForEach(x => x.DeselectCell());

    private void OnDestroy()
    {
        defencesViewModel.OnDefenseSelected -= SelectDefence;
        defencesViewModel.OnDefenseDeselected -= DeselectDefense;
    }
}
