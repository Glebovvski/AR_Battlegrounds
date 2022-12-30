using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Defence_", menuName = "ScriptableDefence", order = 0)]
public class ScriptableDefence : ScriptableObject 
{
    public int Health;
    public bool IsActiveDefence;
    public int AttackForce;
    public int AttackRange;
    public int RelaodTime;
}
