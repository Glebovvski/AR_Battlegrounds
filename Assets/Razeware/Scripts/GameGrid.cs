using System;
using System.Collections.Generic;
using System.Linq;
using Defendable;
using UnityEngine;
using UnityEngine.AI;

public class GameGrid : MonoBehaviour
{
    private const float yPos = -0.99f;

    [SerializeField] private DefencesViewModel defencesViewModel;
    [SerializeField] private DefencesModel defencesModel;
    [SerializeField] private NavMeshSurface plane;

    [SerializeField] private GridCell gridCellPrefab;
    [SerializeField] private CastleDefence castlePrefab;
    private int width = 15;
    private int length = 15;
    private float gridSpaceSize = 1f;

    public List<GridCell> GridList = new List<GridCell>();

    private GridCell[,] grid;

    private List<GridCell> centreCells;

    private List<List<GridCell>> pairCells = new List<List<GridCell>>();

    public PoolObjectType SelectedDefence { get; private set; }

    public Vector3 Centre => this.transform.position + new Vector3(width / 2f, 0, length / 2f);
    public Vector3 Position => this.transform.position;

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

        return new Vector3(x, cell.Height + yPos, y);
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

        return new Vector3(x, height + yPos, y);
    }

    public void SelectDefence(PoolObjectType type)
    {
        SelectedDefence = type;
        var defense = PoolManager.Instance.GetFromPool<Defense>(type);
        var defenseInfo = defense.SO;
        PoolManager.Instance.ReturnToPool(defense.gameObject, type);
        DeselectAllCells();
        if (defenseInfo.Size == Vector2.one)
        {
            var match = GridList.FindAll(defenseInfo.GetCondition()).FindAll(ConditionsData.IsEmptyCell);
            match.ForEach(x => x.SetSelected());
        }
        else
        {
            var pairs = GetCellGroupsBySize(defenseInfo.Size);
            pairCells = new List<List<GridCell>>();
            foreach (var pair in pairs)
            {
                // pair = pair.FindAll(defenseInfo.GetCondition()).ToList();
                if (pair.All(ConditionsData.IsEmptyCell))
                    pairCells.Add(pair);
            }
            foreach (var pair in pairCells)
            {
                foreach (var cell in pair)
                {
                    cell.SetSelected();
                }
            }
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

        var defence = PoolManager.Instance.GetFromPool<Defense>(SelectedDefence);
        if (defence.GetSize() == Vector2Int.one)
        {
            defence.transform.position = GetWorldPositionFromGrid(cell);
            cell.SetDefence(defence);
        }
        else
        {
            var selectedPair = pairCells.First(x => x.Contains(cell));
            if (selectedPair == null) return;

            var centre = GetCentreOfPair(selectedPair);
            defence.transform.position = GetWorldPositionFromGrid(centre, selectedPair[0].Height);
            selectedPair.ForEach(x => x.SetDefence(defence));
        }
        SelectDefence(SelectedDefence);
        if (!cell.IsUpper)
            RebuildNavMesh();
    }

    private List<List<GridCell>> GetCellGroupsBySize(Vector2Int size)
    {
        List<List<GridCell>> cells = new List<List<GridCell>>();
        for (int i = 0; i < width - 1; i++)
        {
            for (int j = 0; j < length - 1; j++)
            {
                var possiblePairs = new List<GridCell>();
                possiblePairs.Add(grid[i, j]);
                possiblePairs.Add(grid[i, j + 1]);
                possiblePairs.Add(grid[i + 1, j]);
                possiblePairs.Add(grid[i + 1, j + 1]);

                if (possiblePairs.All(pair => !pair.IsUpper) || possiblePairs.All(pair => pair.IsUpper))
                    cells.Add(possiblePairs);
            }
        }
        return cells;
    }

    private void CreateGrid()
    {
        grid = new GridCell[width, length];
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < length; z++)
            {
                grid[x, z] = Instantiate(gridCellPrefab, new Vector3(x * gridSpaceSize, yPos, z * gridSpaceSize), Quaternion.Euler(90, 0, 0), this.transform);
                grid[x, z].Init(x, z);
                grid[x, z].gameObject.name = string.Format("Cell {0}:{1}", x, z);
                grid[x, z].OnFreeCell += RebuildNavMesh;
                GridList.Add(grid[x, z]);
            }
        }
        TryChangeHeight();
        SpawnCastleAtCentre();
        RebuildNavMesh();
    }

    private void RebuildNavMesh() => plane.BuildNavMesh();

    public List<Vector3> Corners()
    {
        return new List<Vector3>()
        {
            this.transform.position,
            this.transform.position + new Vector3(width, 0, 0),
            this.transform.position + new Vector3(width,0, length),
            this.transform.position + new Vector3(0, 0, length)
        };
    }

    private void SpawnCastleAtCentre()
    {
        Vector2 centre = GetCentreOfPair(centreCells);
        var castle = Instantiate(castlePrefab, GetWorldPositionFromGrid(centre, centreCells[0].Height), Quaternion.identity);
        centreCells.ForEach(x => x.SetDefence(castle));
    }

    private Vector2 GetCentreOfPair(List<GridCell> cells) => cells.Aggregate(Vector2.zero, (acc, v) => acc + v.Pos) / cells.Count;

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
