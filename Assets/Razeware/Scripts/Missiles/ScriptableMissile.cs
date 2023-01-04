using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Missile_", menuName = "ScriptableMissile", order = 0)]
public class ScriptableMissile : ScriptableObject
{
    public int Damage;
    public float Speed;
    public bool IsFromPool;
}
