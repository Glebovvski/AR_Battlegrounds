using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private CharacterMain player;
    [SerializeField] private Transform target;

    public void Update()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit))
        {
            target.position = hit.point;
            player.SetDestination(target.position);
        }
    }

}
