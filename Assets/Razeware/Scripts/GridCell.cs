using System;
using Defendable;
using UnityEngine;
using UnityEngine.AI;

public class GridCell : MonoBehaviour
{
    [SerializeField] private MeshRenderer quadRenderer;
    [SerializeField] private MeshFilter cubeMesh;
    [SerializeField] private Mesh ground;
    [SerializeField] private Mesh grass;
    [SerializeField] private NavMeshSurface surface;

    private int posX;
    private int posY;

    public Defense Defence { get; private set; }
    public bool IsFree => Defence == null;
    public Vector2Int Pos => new Vector2Int(posX, posY);
    public int Height => Mathf.FloorToInt(transform.localScale.z);
    public bool IsUpper => transform.localScale.z > 1;
    public bool IsSelected => quadRenderer.material.color != defaultCellColor;

    private Color defaultCellColor;


    public void SetSelected() => quadRenderer.material.color = Color.green * 10;
    public void Init(int x, int y)
    {
        posX = x;
        posY = y;
    }

    public void SetHeight(int value)
    {
        this.transform.localScale = new Vector3(1, 1, value);
        cubeMesh.mesh = value == 1 ? grass : ground;

    }
    public void SetDefence(Defense defence)
    {
        if (IsFree)
        {
            Defence = defence;
        }
    }

    public void BuildNavMesh() => surface.BuildNavMesh();

    internal void DeselectCell() => quadRenderer.material.color = defaultCellColor;
}
