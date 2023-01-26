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

    [SerializeField] private PlaneManager plane;

    [SerializeField] private GridCell gridCellPrefab;

    [field: SerializeField] public int Width { get; private set; }
    [field: SerializeField] public int Length { get; private set; }

    private float gridSpaceSize = 1;

    public List<GridCell> GridList = new List<GridCell>();

    private List<List<GridCell>> grid;

    private List<GridCell> centreCells;

    private List<List<GridCell>> pairCells = new List<List<GridCell>>();

    public ScriptableDefense SelectedDefense { get; private set; }

    public Vector3 Centre => this.transform.position + new Vector3(Width / 2f, 0, Length / 2f);
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

        gridSpaceSize *= plane.transform.localScale.x;

        // CreateGrid();
    }

    public Vector2Int GetGridPositionFromWorld(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt(worldPosition.x / gridSpaceSize);
        int y = Mathf.FloorToInt(worldPosition.z / gridSpaceSize);

        x = Mathf.Clamp(x, 0, Width);
        y = Mathf.Clamp(y, 0, Length);

        return new Vector2Int(x, y);
    }

    public Vector3 GetWorldPositionFromGrid(GridCell cell)
    {
        float x = cell.Pos.x * gridSpaceSize;
        float y = cell.Pos.y * gridSpaceSize;

        return new Vector3(x, cell.Height + yPos, y);
    }

    public Vector3 GetWorldPositionFromGrid(Vector2Int position, float height)
    {
        float x = position.x * gridSpaceSize;
        float y = position.y * gridSpaceSize;

        return new Vector3(x, height, y);
    }

    public Vector3 GetWorldPositionFromGrid(Vector2 position, float height)
    {
        float x = position.x * gridSpaceSize;
        float y = position.y * gridSpaceSize;

        // return new Vector3(x, height + yPos, y);
        return new Vector3(x, height, y);
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

        var defense = PoolManager.Instance.GetFromPool<Defense>(SelectedDefense.PoolType);
        defense.Init(SelectedDefense);
        plane.AttachChild(defense.transform);
        if (defense.GetSize() == Vector2Int.one)
        {
            var position = new Vector2(cell.transform.position.x, cell.transform.position.z);
            defense.transform.position = GetWorldPositionFromGrid(position, cell.Height);
            cell.SetDefence(defense);
        }
        else
        {
            var selectedPair = pairCells.First(x => x.Contains(cell));
            if (selectedPair == null) return;

            var centre = GetCentreOfPair(selectedPair);
            defense.transform.position = GetWorldPositionFromGrid(centre, selectedPair[0].Height);
            selectedPair.ForEach(x => x.SetDefence(defense));
        }
        CurrencyModel.Buy(SelectedDefense.Price);
        if (CurrencyModel.Gold >= SelectedDefense.Price)
            SelectDefence(SelectedDefense);
    }

    private List<List<GridCell>> GetCellGroupsBySize(Vector2Int size)
    {
        List<List<GridCell>> cells = new List<List<GridCell>>();
        if (gridType == GridType.Rectangle)
        {
            for (int i = 0; i < Width - 1; i++)
            {
                for (int j = 0; j < Length - 1; j++)
                {
                    var possiblePairs = new List<GridCell>();

                    possiblePairs.Add(grid[i][j]);
                    possiblePairs.Add(grid[i][j + 1]);
                    possiblePairs.Add(grid[i + 1][j]);
                    possiblePairs.Add(grid[i + 1][j + 1]);

                    if (possiblePairs.All(pair => !pair.IsUpper))
                        cells.Add(possiblePairs);
                }
            }
        }
        else
        {
            for (int i = 0; i < grid.Count - 1; i++)
            {
                for (int j = 0; j < grid[i].Count - 1; j++)
                {
                    var possiblePairs = new List<GridCell>();

                    int nextIndex = Mathf.Abs(grid[i].Count - grid[i + 1].Count) / 2;
                    int sign = grid[i].Count > grid[i + 1].Count ? 1 : -1;
                    if (grid[i].Count < 2) continue;
                    if (!grid[i][j].IsFree) continue;
                    if (grid[i][j].IsUpper) continue;

                    possiblePairs.Add(grid[i][j]);
                    possiblePairs.Add(grid[i][j + 1]);

                    if (j - sign * nextIndex < 0 || j - sign * nextIndex >= grid[i + 1].Count)
                        continue;
                    possiblePairs.Add(grid[i + 1][j - sign * nextIndex]);
                    if (grid[i + 1].Count <= j - sign * nextIndex + 1)
                        continue;
                    possiblePairs.Add(grid[i + 1][j - sign * nextIndex + 1]);

                    if (possiblePairs.All(pair => !pair.IsUpper))
                        cells.Add(possiblePairs);
                }
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
                case (GridType.Ellipse):
                    CreateEllipseGrid();
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
        for (int x = -Width / 2; x < Width / 2; x++)
        {
            int x_index = x + Width / 2;
            var z_list = new List<GridCell>();
            for (int z = -Length / 2; z < Length / 2; z++)
            {
                var cell = Instantiate(gridCellPrefab, new Vector3(x * gridSpaceSize, 0, z * gridSpaceSize), Quaternion.identity, this.transform);
                cell.Init(x, z);
                cell.gameObject.name = string.Format("Cell {0}:{1}", x, z);
                cell.OnFreeCell += RebuildNavMesh;
                z_list.Add(cell);
            }
            grid.Add(z_list);
            GridList.AddRange(grid[x_index]);
        }
    }

    private void CreateCircleGrid()
    {
        int radius = Width / 2;

        int square = radius * radius;

        grid = new List<List<GridCell>>();

        for (int x = -radius; x <= radius; ++x)
        {
            int x_index = x + radius;
            var z_list = new List<GridCell>();
            for (int z = -radius; z <= radius; ++z)
            {
                if (new Vector2(x, z).sqrMagnitude > square) continue;

                var cell = Instantiate(gridCellPrefab, new Vector3(x * gridSpaceSize, 0, z * gridSpaceSize), Quaternion.identity, this.transform);
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

    private void CreateEllipseGrid()
    {
        int x_radius = Width / 2;
        int z_radius = Length / 2;
        int x_square = x_radius * x_radius;
        int z_square = z_radius * z_radius;

        grid = new List<List<GridCell>>();

        for (int x = -x_radius; x <= x_radius; ++x)
        {
            int x_index = x + x_radius;
            var z_list = new List<GridCell>();
            for (int z = -z_radius; z <= z_radius; ++z)
            {
                if ((z_square * x * x + x_square * z * z) > (x_square * z_square)) continue;

                var cell = Instantiate(gridCellPrefab, new Vector3(x * gridSpaceSize, 0, z * gridSpaceSize), Quaternion.identity, this.transform);
                int z_index = z + z_radius;
                cell.Init(x_index, z_index);
                cell.gameObject.name = string.Format("Cell {0}:{1}", x_index, z_index);
                cell.OnFreeCell += RebuildNavMesh;
                z_list.Add(cell);
            }
            grid.Add(z_list);
            GridList.AddRange(grid[x_index]);
        }
    }

    public void RebuildNavMesh()
    {
        // plane.BuildNavMesh();
    }

    public List<Vector3> Corners()
    {
        return new List<Vector3>()
        {
            this.transform.position,
            this.transform.position + new Vector3(Width, 0, 0),
            this.transform.position + new Vector3(Width,0, Length),
            this.transform.position + new Vector3(0, 0, Length)
        };
    }

    private void SpawnCastleAtCentre()
    {
        Vector2 centre = GetCentreOfPair(centreCells);
        Castle.Init(DefensesModel.List.Where(x => x.Type == DefenseType.Castle).FirstOrDefault());
        Castle.transform.position = GetWorldPositionFromGrid(centre, 1);
        plane.AttachChild(Castle.transform);
        centreCells.ForEach(x => x.SetDefence(Castle));
    }

    private Vector2 GetCentreOfPair(List<GridCell> cells) => cells.Aggregate(Vector2.zero, (acc, v) => acc + new Vector2(v.transform.position.x, v.transform.position.z)) / cells.Count;

    private void TryChangeHeight()
    {
        int centreX = Width / 2 - 1;
        int centreY = Length / 2 - 1;
        var gridCell1 = grid[centreX][centreY];
        var gridCell2 = gridType == GridType.Rectangle ? grid[centreX][centreY + 1] : grid[centreX][centreY - 1];
        var gridCell3 = grid[centreX + 1][centreY];
        var gridCell4 = grid[centreX + 1][centreY + 1];

        centreCells = new List<GridCell>()
        {
            gridCell1, gridCell2, gridCell3, gridCell4
        };

        foreach (var cell in GridList)
        {
            if (IsMustHaveGroundHeight(cell))
            {
                // SelectedDefense = DefensesModel.List.First(x => x.PoolType == PoolObjectType.WallTower);
                // SpawnDefence(cell);
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
                   cell.Pos.x == -Width / 2
                || cell.Pos.y == -Length / 2
                || cell.Pos.x == Width / 2 - 1
                || cell.Pos.y == Length / 2 - 1
                || centreCells.Contains(cell);
            case GridType.Circle:
                bool isLastRowCircle = grid.Last().Contains(cell);
                bool isFirstRowCircle = grid.First().Contains(cell);
                var square_circle = (Width / 2) * (Width / 2);
                bool isLastInColumnCircle = new Vector2(cell.Pos.x - Width / 2, cell.Pos.y - Length / 2).sqrMagnitude > square_circle - 2 * Width * gridSpaceSize;
                return
                   centreCells.Contains(cell)
                   || isLastRowCircle
                   || isFirstRowCircle
                   || isLastInColumnCircle;
            case GridType.Ellipse:
                bool isLastRowEllipse = grid.Last().Contains(cell);
                bool isFirstRowEllipse = grid.First().Contains(cell);
                if (isFirstRowEllipse || isLastRowEllipse)
                    return true;

                var index = grid[cell.Pos.x].IndexOf(cell);
                bool isFirstOrLastInColumn = index == 0 || index == grid[cell.Pos.x].Count - 1;
                int prevIndex = Mathf.Abs((grid[cell.Pos.x].Count - grid[cell.Pos.x - 1].Count) / 2);
                int nextIndex = Mathf.Abs((grid[cell.Pos.x].Count - grid[cell.Pos.x + 1].Count) / 2);
                int sign = grid[cell.Pos.x - 1].Count > grid[cell.Pos.x].Count ? 1 : -1;
                bool isPrevIndexNotExist = grid[cell.Pos.x - 1].Count < cell.Pos.y + prevIndex * sign;
                bool isNextIndexNotExist = grid[cell.Pos.x + 1].Count < cell.Pos.y + nextIndex * sign;
                if (isNextIndexNotExist || isPrevIndexNotExist)
                    return true;

                bool isPrevLowerIndexNotExist = index + prevIndex * sign - 1 < 0; //diagonal left lower check
                bool isPrevHigherIndexNotExist = index + prevIndex * sign + 1 >= grid[cell.Pos.x - 1].Count; //diagonal left higher check
                bool isNextLowerIndexNotExist = index + nextIndex * sign - 1 < 0; //diagonal right lower check
                bool isNextHigherIndexNotExist = index + prevIndex * sign + 1 >= grid[cell.Pos.x + 1].Count; //diagonal right higher check

                return
                    centreCells.Contains(cell)
                    || isLastRowEllipse
                    || isFirstRowEllipse
                    || isFirstOrLastInColumn
                    || isPrevIndexNotExist
                    || isNextIndexNotExist
                    || isPrevLowerIndexNotExist
                    || isPrevHigherIndexNotExist
                    || isNextLowerIndexNotExist
                    || isNextHigherIndexNotExist;

            default:
                return true;

        }
    }

    private bool IsAnyDiagonalCellUp(GridCell cell)
    {
        int x_index = gridType == GridType.Rectangle ? cell.Pos.x + Width / 2 : cell.Pos.x;
        int y_index = grid[x_index].IndexOf(cell);

        int prevIndex = Mathf.Abs((grid[x_index].Count - grid[x_index - 1].Count) / 2);
        int nextIndex = Mathf.Abs((grid[x_index].Count - grid[x_index + 1].Count) / 2);
        int sign = grid[x_index - 1].Count > grid[x_index].Count ? 1 : -1;
        return
               grid[x_index - 1][y_index + prevIndex * sign - 1].IsUpper
            || grid[x_index - 1][y_index + prevIndex * sign + 1].IsUpper
            || grid[x_index + 1][y_index + nextIndex * sign + 1].IsUpper
            || grid[x_index + 1][y_index + nextIndex * sign - 1].IsUpper;
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
    Ellipse = 2,
}
