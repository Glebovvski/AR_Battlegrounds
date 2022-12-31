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

    public Predicate<GridCell> GetCondition(Conditions condition) => ConditionData.FirstOrDefault(x => x.Name == condition).Condition;

    private List<ConditionData<GridCell>> ConditionData = new List<ConditionData<GridCell>>()
    {
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

    public static bool HasZeroHeight(GridCell cell) => !cell.IsUpper;
    public static bool HasNonZeroHeight(GridCell cell) => cell.IsUpper;

}

[Serializable]
public enum Conditions
{
    HasZeroHeight = 0,
    HasNonZeroHeight = 1,
}

[Serializable]
public class ConditionData<T>
{
    public Conditions Name;
    public Predicate<T> Condition;
}
