using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Enemy_", menuName = "ScriptableEnemy", order = 0)]
public class ScriptableEnemy : ScriptableObject
{
    public int Health;
    public int AttackForce;
    public int GoldToDrop;
    public int Speed;
    public float ScanRange;
    public float AttackRange;

    [Space(10)]
    public int AttackWallScore;
    public int AttackCannonScore;
    public int AttackLaserScore;
    public int AttackCastleScore;
    [Space(10)]
    public PoolObjectType Type;
}
