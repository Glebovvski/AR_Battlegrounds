using UnityEngine;
using Zenject;

public class DecorManager : MonoBehaviour
{
    private GameGrid Grid { get; set; }

    [Inject]
    private void Construct(GameGrid grid)
    {
        Grid = grid;
    }

    private void Awake()
    {
        Grid.OnGridCreated += SetDecor;
    }

    private void SetDecor()
    {
        var decors = PoolManager.Instance.decorList;
        Vector3 position = Vector3.zero;
        foreach (var decorInfo in decors)
        {
            int count = Random.Range(1, decorInfo.amount + 1);
            for (int i = 0; i < count; i++)
            {
                var decor = PoolManager.Instance.GetFromDecor(decorInfo.type);
                do
                {
                    position = new Vector3(
                        Random.Range(this.transform.position.x - this.transform.localScale.x * 5, this.transform.position.x + this.transform.localScale.x * 5),
                        0,
                        Random.Range(this.transform.position.z - this.transform.localScale.z * 5, this.transform.position.z + this.transform.localScale.z * 5)
                    );
                } while (IsOccupied(position));

                decor.transform.position = position;
            }
        }
    }

    private bool IsOccupied(Vector3 position)
    {
        LayerMask mask = LayerMask.GetMask("Grid");
        Collider[] colliders = Physics.OverlapSphere(position, 5, mask);
        return colliders.Length > 1;

    }
}
