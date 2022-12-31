using Defendable;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    [SerializeField] private MeshRenderer renderer;

    private int posX;
    private int posY;

    public Defence Defence { get; private set; }
    public bool IsFree => Defence == null;
    public Vector2Int Position => new Vector2Int(posX, posY);
    public bool IsUpper => transform.localScale.z > 1;


    public void SetSelected() => renderer.material.color = Color.green * 10;
    public void Init(int x, int y)
    {
        posX = x;
        posY = y;
    }

    public void SetHeight(int value) => this.transform.localScale = new Vector3(1, 1, value);

    public void SetDefence(Defence defence)
    {
        if (IsFree)
            Defence = defence;
    }
}
