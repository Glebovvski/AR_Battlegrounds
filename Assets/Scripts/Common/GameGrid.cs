using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Defendable;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class GameGrid : MonoBehaviour
{
    private const float yPos = -0.99f;

    private CurrencyModel CurrencyModel { get; set; }
    private DefensesModel DefensesModel { get; set; }
    private CastleDefense Castle { get; set; }
    private LoseModel LoseModel { get; set; }

    [SerializeField] private GridType gridType;

    [SerializeField] private DefensesViewModel defencesViewModel;

    [SerializeField] private NavMeshSurface plane;
    [SerializeField] private NavMeshSurface gridSurface;
    [SerializeField] private Transform planeObstacle;

    [SerializeField] private GridCell gridCellPrefab;

    [SerializeField] private int width;
    [SerializeField] private int length;

    private float gridSpaceSize = 1f;

    public List<GridCell> GridList = new List<GridCell>();

    // private GridCell[,] grid;
    private List<List<GridCell>> grid;

    private List<GridCell> centreCells;

    private List<List<GridCell>> pairCells = new List<List<GridCell>>();

    public ScriptableDefense SelectedDefense { get; private set; }

    public Vector3 Centre => this.transform.position + new Vector3(width / 2f, 0, length / 2f);
    public Vector3 Position => this.transform.position;

    public event Action OnGridCreated;

    [Inject]
    private void Construct(CurrencyModel currencyModel, DefensesModel defensesModel, CastleDefense castleDefense, LoseModel loseModel)
    {
        CurrencyModel = currencyModel;
        DefensesModel = defensesModel;
        Castle = castleDefense;
        LoseModel = loseModel;
    }

    // Start is called before the first frame update
    void Start()
    {
        DefensesModel.OnDefenseSelected += SelectDefence;
        DefensesModel.OnDefenseDeselected += DeselectDefense;
        LoseModel.OnRestart += RemoveAllDefenses;

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
        // return new Vector3(x, 0, y);
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
        // return new Vector3(x, 0, y);
    }

    public void SelectDefence(ScriptableDefense info)
    {
        SelectedDefense = info;
        DeselectAllCells();
        if (info.Size == Vector2.one)
        {
            var match = GridList.FindAll(info.GetCondition()).FindAll(ConditionsData.IsEmptyCell);
            match.ForEach(x => x.SetSelected());
        }
        else
        {
            var pairs = GetCellGroupsBySize(info.Size);
            pairCells = new List<List<GridCell>>();
            foreach (var pair in pairs)
            {
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
        SelectedDefense = null;
    }

    public void SpawnDefence(GridCell cell)
    {
        if (SelectedDefense == null) return;

        var defence = PoolManager.Instance.GetFromPool<Defense>(SelectedDefense.PoolType);
        defence.Init(SelectedDefense);
        defence.transform.SetParent(plane.transform);
        if (defence.GetSize() == Vector2Int.one)
        {
            defence.transform.position = GetWorldPositionFromGrid(cell.Pos, cell.IsUpper ? Mathf.CeilToInt(-yPos) : Mathf.FloorToInt(-yPos));
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
        CurrencyModel.Buy(SelectedDefense.Price);
        if (CurrencyModel.Gold >= SelectedDefense.Price)
            SelectDefence(SelectedDefense);
    }

    private List<List<GridCell>> GetCellGroupsBySize(Vector2Int size)
    {
        List<List<GridCell>> cells = new List<List<GridCell>>();
        for (int i = 0; i < width - 1; i++)
        {
            for (int j = 0; j < length - 1; j++)
            {
                var possiblePairs = new List<GridCell>();
                possiblePairs.Add(grid[i][j]);
                possiblePairs.Add(grid[i][j + 1]);
                possiblePairs.Add(grid[i + 1][j]);
                possiblePairs.Add(grid[i + 1][j + 1]);

                if (possiblePairs.All(pair => !pair.IsUpper) || possiblePairs.All(pair => pair.IsUpper))
                    cells.Add(possiblePairs);
            }
        }
        return cells;
    }

    public void CreateGrid()
    {
        if (grid == null)
        {
            switch (gridType)
            {
                case (GridType.Rectangle):
                    CreateRectangleGrid();
                    break;
                case (GridType.Circle):
                    CreateCircleGrid();
                    break;
            }
        }
        else
            GridList.ForEach(cell => cell.SetHeight(1));

        TryChangeHeight();
        SpawnCastleAtCentre();
        RebuildNavMesh();
        OnGridCreated?.Invoke();
    }

    private void CreateRectangleGrid()
    {
        grid = new List<List<GridCell>>();
        for (int x = 0; x < width; x++)
        {
            var z_list = new List<GridCell>();
            for (int z = 0; z < length; z++)
            {
                var cell = Instantiate(gridCellPrefab, new Vector3(x * gridSpaceSize, yPos, z * gridSpaceSize), Quaternion.identity, this.transform);
                cell.Init(x, z);
                cell.gameObject.name = string.Format("Cell {0}:{1}", x, z);
                cell.OnFreeCell += RebuildNavMesh;
                z_list.Add(cell);
            }
            grid.Add(z_list);
            GridList.AddRange(grid[x]);
        }
    }

    private void CreateCircleGrid()
    {
        int radius = width / 2;

        int square = radius * radius;

        grid = new List<List<GridCell>>();

        for (int x = -radius; x <= radius; ++x)
        {
            int x_index = x + radius;
            var z_list = new List<GridCell>();
            for (int z = -radius; z <= radius; ++z)
            {
                if (new Vector2(x, z).sqrMagnitude > square) continue;

                var cell = Instantiate(gridCellPrefab, new Vector3(x * gridSpaceSize, yPos, z * gridSpaceSize), Quaternion.identity, this.transform);
                int z_index = z + radius;
                cell.Init(x_index, z_index);
                cell.gameObject.name = string.Format("Cell {0}:{1}", x_index, z_index);
                cell.OnFreeCell += RebuildNavMesh;
                z_list.Add(cell);
            }
            grid.Add(z_list);
            GridList.AddRange(grid[x_index]);
        }
    }

    private void RebuildNavMesh()
    {
        plane.BuildNavMesh();
    }

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
        Castle.Init(DefensesModel.List.Where(x => x.Type == DefenseType.Castle).FirstOrDefault());
        Castle.transform.position = GetWorldPositionFromGrid(centre, centreCells[0].Height);
        Castle.transform.SetParent(plane.transform);
        centreCells.ForEach(x => x.SetDefence(Castle));
    }

    private Vector2 GetCentreOfPair(List<GridCell> cells) => cells.Aggregate(Vector2.zero, (acc, v) => acc + v.Pos) / cells.Count;

    private void TryChangeHeight()
    {
        var gridCell1 = grid[width / 2][length / 2];
        var gridCell2 = grid[gridCell1.Pos.x][gridCell1.Pos.y - 1];
        var gridCell3 = grid[gridCell1.Pos.x + 1][gridCell1.Pos.y];
        var gridCell4 = grid[gridCell1.Pos.x + 1][gridCell1.Pos.y - 1];
        centreCells = new List<GridCell>()
        {
            gridCell1, gridCell2, gridCell3, gridCell4
        };

        foreach (var cell in GridList)
        {
            if (IsMustHaveGroundHeight(cell))
            {
                // if (!centreCells.Contains(cell))
                // {
                //     SelectedDefense = DefensesModel.List.Where(x => x.PoolType == PoolObjectType.WallTower).FirstOrDefault();
                //     SpawnDefence(cell);
                // }
                continue;
            }
            DeselectAllCells();
            if (IsAnyDiagonalCellUp(cell))
                continue;
            cell.SetHeight(UnityEngine.Random.Range(1, 3));
        }

    }

    private bool IsMustHaveGroundHeight(GridCell cell)
    {
        switch (gridType)
        {
            case (GridType.Rectangle):
                return
                   cell.Pos.x == 0
                || cell.Pos.y == 0
                || cell.Pos.x == width - 1
                || cell.Pos.y == length - 1
                || centreCells.Contains(cell);
            case GridType.Circle:
                var firstX = grid[cell.Pos.x].First();
                var lastX = grid[cell.Pos.x].Last();
                // var firstY = grid.First()[cell.Pos.y];
                // var lastY = grid.Last()[cell.Pos.y];
                try
                {
                    return
                       centreCells.Contains(cell)
                    || firstX == cell
                    || lastX == cell;
                    // || firstY == cell
                    // || lastY == cell;
                }
                catch
                {
                    Debug.LogError(cell.name);
                    return true;
                }
            default:
                return false;

        }
    }

    private bool IsAnyDiagonalCellUp(GridCell cell)
    {
        return 
           grid[cell.Pos.x - 1][cell.Pos.y - 1].IsUpper
        || grid[cell.Pos.x - 1][cell.Pos.y + 1].IsUpper
        || grid[cell.Pos.x + 1][cell.Pos.y + 1].IsUpper 
        || grid[cell.Pos.x + 1][cell.Pos.y - 1].IsUpper;
    }


    public void RemoveAllDefenses()
    {
        foreach (var cell in GridList)
        {
            if (cell.IsFree) continue;
            if (cell.Defence.Type != DefenseType.Castle)
            {
                PoolManager.Instance.ReturnToPool(cell.Defence.gameObject, cell.Defence.DefenseTypeToPoolType(cell.Defence.Type));
                cell.FreeCell();
            }
        }
    }

    private void DeselectAllCells() => GridList.ForEach(x => x.DeselectCell());

    private void OnDestroy()
    {
        DefensesModel.OnDefenseSelected -= SelectDefence;
        DefensesModel.OnDefenseDeselected -= DeselectDefense;
        LoseModel.OnRestart -= RemoveAllDefenses;
    }
}

public enum GridType
{
    Rectangle = 0,
    Circle = 1,
    Star = 2,
}
