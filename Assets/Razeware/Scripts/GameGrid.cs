using System;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    [SerializeField] private GridCell gridCellPrefab;
    private int width = 15;
    private int length = 15;
    private float gridSpaceSize = 1f;

    public List<GridCell> GridList = new List<GridCell>();

    private GridCell[,] grid;

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
                TryChangeHeight(grid[x, z]);
                grid[x, z].gameObject.name = string.Format("Cell {0}:{1}", x, z);
                grid[x, z].transform.parent = transform;
                GridList.Add(grid[x,z]);
            }
        }
    }

    private void TryChangeHeight(GridCell cell)
    {
        if (cell.Position.x == 0 || cell.Position.y == 0 || cell.Position.x == width - 1 || cell.Position.y == length - 1)
            return;

        cell.SetHeight(UnityEngine.Random.Range(1, 3));
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
        float x = position.x * gridSpaceSize;
        float y = position.y * gridSpaceSize;

        return new Vector3(x, 0, y);
    }
}
