using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "SO_Defence_", menuName = "ScriptableDefence", order = 0)]
public class ScriptableDefense : ScriptableObject
{
    public string Name;
    public Image Image;
    public int Price;

    [Space(10)]
    public int Health;
    public bool IsActiveDefence;
    public int AttackForce;
    public int AttackRange;
    public int RelaodTime;
    public float ProjectileSpeed;
    public Vector2Int Size;
    public Conditions Condition;
    public PoolObjectType PoolType;
    public DefenseType Type;

    public Predicate<GridCell> GetCondition() => ConditionsData.ConditionData.FirstOrDefault(x => x.Name == Condition).Condition;
}

public enum DefenseType
{
    None,
    Castle = 1,
    Wall = 2,
    Cannon = 3,
    Laser = 4,
    Trap = 5,
    MissileLauncher = 6,
}

public static class ConditionsData
{
    private static bool HasZeroHeight(GridCell cell) => !cell.IsUpper;
    private static bool HasNonZeroHeight(GridCell cell) => cell.IsUpper;
    private static bool EmptyCondition(GridCell cell) => true; 
    public static bool IsEmptyCell(GridCell cell) => cell.IsFree;
    public static readonly List<ConditionData<GridCell>> ConditionData = new List<ConditionData<GridCell>>()
    {
        new ConditionData<GridCell>()
        {
            Name = Conditions.None,
            Condition = EmptyCondition
        },
        new ConditionData<GridCell>()
        {
            Name = Conditions.HasZeroHeight,
            Condition = HasZeroHeight
        },
        new ConditionData<GridCell>()
        {
            Name = Conditions.HasNonZeroHeight,
            Condition = HasNonZeroHeight
        }
    };
}

[Serializable]
public enum Conditions
{
    None = 0,
    HasZeroHeight = 1,
    HasNonZeroHeight = 2,
}

[Serializable]
public class ConditionData<T>
{
    public Conditions Name;
    public Predicate<T> Condition;
}
