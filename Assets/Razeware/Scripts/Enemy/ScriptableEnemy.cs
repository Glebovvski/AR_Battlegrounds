using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Enemy_", menuName = "ScriptableEnemy", order = 0)]
public class ScriptableEnemy : ScriptableObject
{
    public int Health;
    public int AttackForce;
    public int GoldToDrop;
}
