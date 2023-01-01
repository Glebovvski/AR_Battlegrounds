using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Missile : MonoBehaviour
{
    public Vector3 Direction {get; private set;} = Vector3.zero;
    public void Fire(Vector3 direction)
    {
        Direction = direction;
    }

    private void Update() 
    {
        transform.position += Direction*Time.deltaTime;    
    }
}
