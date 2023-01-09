using AI;
using Apex.AI;
using Apex.Serialization;

public class IsTankInRange : ContextualScorerBase
{
    [ApexSerialization] private bool not;
    public override float Score(IAIContext context)
    {
        var c = (AIContext)context;
        var enemy = c.Enemy;

        var tank = Enemies.AIManager.Instance.GetClosestEnemyByType(enemy, Enemies.EnemyType.Mono);
        if (tank != null)
            return not ? -100 : 100;
        else
            return not ? 100 : -100;
    }
}
