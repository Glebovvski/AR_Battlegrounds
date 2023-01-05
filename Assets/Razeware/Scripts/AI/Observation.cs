using System;
using Defendable;
using UnityEngine;

[Serializable]
public class Observation :  IEquatable<Observation>
{
    public Observation(Defense defence)
    {
        this.Defense = defence;
    }

    public Defense Defense { get; private set; }
    public Vector3 Position => Defense.gameObject.transform.position;
    public bool IsAlive => Defense != null;

    public bool Equals(Observation other)
    {
        return this.Defense == other.Defense;
    }
}
