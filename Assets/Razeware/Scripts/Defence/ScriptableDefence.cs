using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "SO_Defence_", menuName = "ScriptableDefence", order = 0)]
public class ScriptableDefence : ScriptableObject
{
    public int Health;
    public bool IsActiveDefence;
    public int AttackForce;
    public int AttackRange;
    public int RelaodTime;
    public Vector2 Size;
    public Conditions Condition;

    public Predicate<GridCell> GetCondition() => ConditionsData.ConditionData.FirstOrDefault(x => x.Name == Condition).Condition;
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
