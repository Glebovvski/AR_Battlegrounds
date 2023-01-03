using System.Collections;
using System.Collections.Generic;
using Defendable;
using UnityEngine;

public class Observation
{
    public Observation(Defence defence)
    {
        this.Defence = defence;
    }

    public Defence Defence { get; private set; }
    public Vector3 Position => Defence.gameObject.transform.position;
}
