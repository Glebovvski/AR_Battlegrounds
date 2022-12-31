using System;
using System.Collections.Generic;
using System.Linq;
using Defendable;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    [SerializeField] private GridCell gridCellPrefab;
    [SerializeField] private CastleDefence castlePrefab;
    private int width = 15;
    private int length = 15;
    private float gridSpaceSize = 1f;

    public List<GridCell> GridList = new List<GridCell>();

    private GridCell[,] grid;

    private List<GridCell> centreCells;

    // Start is called before the first frame update
    void Start()
    {
        CreateGrid();
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
        var centre = centreCells.Aggregate(Vector2Int.zero, (acc, v) => acc + v.Position) / centreCells.Count;
        var castle = Instantiate(castlePrefab, GetWorldPositionFromGrid(centre) + new Vector3(0, centreCells[0].Height, 0), Quaternion.identity);
        centreCells.ForEach(x => x.SetDefence(castle));
    }

    private void TryChangeHeight(GridCell cell)
    {
        if (cell.Position.x == 0 || cell.Position.y == 0 || cell.Position.x == width - 1 || cell.Position.y == length - 1)
            return;

        cell.SetHeight(UnityEngine.Random.Range(1, 3));
    }

    private void TryChangeHeight()
    {
        int centre = GridList.Count / 2;

        var gridCell1 = GridList[centre];
        var gridCell2 = grid[gridCell1.Position.x, gridCell1.Position.y - 1];
        var gridCell3 = grid[gridCell1.Position.x + 1, gridCell1.Position.y];
        var gridCell4 = grid[gridCell1.Position.x + 1, gridCell1.Position.y - 1];
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
        return cell.Position.x == 0 || cell.Position.y == 0 || cell.Position.x == width - 1 || cell.Position.y == length - 1 || centreCells.Contains(cell);
    }

    private bool IsAnyDiagonalCellUp(GridCell cell)
    {
        return grid[cell.Position.x - 1, cell.Position.y - 1].IsUpper || grid[cell.Position.x - 1, cell.Position.y + 1].IsUpper
                || grid[cell.Position.x + 1, cell.Position.y + 1].IsUpper || grid[cell.Position.x + 1, cell.Position.y - 1].IsUpper;
    }

    public Vector2Int GetGridPositionFromWorld(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt(worldPosition.x / gridSpaceSize);
        int y = Mathf.FloorToInt(worldPosition.z / gridSpaceSize);

        x = Mathf.Clamp(x, 0, width);
        y = Mathf.Clamp(y, 0, length);

        return new Vector2Int(x, y);
    }

    public Vector3 GetWorldPositionFromGrid(Vector2Int position)
    {
        float x = position.x * gridSpaceSize + 0.5f;
        float y = position.y * gridSpaceSize + 0.5f;

        return new Vector3(x, 0, y);
    }
}
