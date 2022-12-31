using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private GameGrid grid;
    [SerializeField] private LayerMask GridLayer;

    private void Update() 
    {
        var cell = GetGridCell();
        if(!cell) return;

        if(Input.GetMouseButtonDown(0))
        {
            cell.SetSelected();
        }
    }

    private GridCell GetGridCell()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out var raycastHit, GridLayer))
        {
            return raycastHit.transform.GetComponent<GridCell>(); 
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
