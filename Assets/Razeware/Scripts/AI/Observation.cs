using System;
using Defendable;
using UnityEngine;

[Serializable]
public class Observation :  IEquatable<Observation>
{
    public Observation(Defense defence)
    {
        this.Defence = defence;
    }

    public Defense Defence { get; private set; }
    public Vector3 Position => Defence.gameObject.transform.position;

    public bool Equals(Observation other)
    {
        return this.Defence == other.Defence;
    }
}
