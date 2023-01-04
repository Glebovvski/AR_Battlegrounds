using System.Collections.Generic;
using System.Linq;
using AI;
using Apex.AI;
using Enemies;

public class GetBestAttackTarget : ActionBase
{
    List<TargetScore> scores = new List<TargetScore>();
    public override void Execute(IAIContext context)
    {
        var c = (AIContext)context;
        var enemy = c.Enemy;

        int multiplier = 100; //Score based on defense type preference
        foreach (var defense in enemy.DefenseTypeToScore)
        {
            var bestDefenseType = defense;
            var closestObservation = Enemies.AIManager.Instance.GetClosestByType(enemy, defense.Key);
            var path = enemy.GetCalculatedPath(closestObservation);
            var enemiesWithSameTarget = Enemies.AIManager.Instance.GetEnemiesAttackingObservation(closestObservation);
            scores.Add(new TargetScore(path.corners.Length, enemiesWithSameTarget, enemy, closestObservation, multiplier));
            multiplier -= 20;
        }
        var attackTarget = scores.OrderByDescending(x => x.TotalScore).First().Observation;
        c.Enemy.AttackTarget = attackTarget.Defence;
    }
}

public class TargetScore
{
    private int pathLengthScore;
    private int enemiesAttackingTarget;
    private Enemy enemy;
    private int extraScore;
    public Observation Observation { get; private set; }

    public int TotalScore => GetTotalScore();

    public TargetScore(int pathLength, int enemiesAttackingTarget, Enemy enemy, Observation observation, int extraScore)
    {
        pathLengthScore = 100 - pathLength;
        this.enemiesAttackingTarget = enemiesAttackingTarget;
        this.enemy = enemy;
        Observation = observation;
        this.extraScore = extraScore;
    }

    public int GetTotalScore()
    {
        return pathLengthScore + GetScoreForEnemiesAttackingTarget() + extraScore;
    }

    private int GetScoreForEnemiesAttackingTarget()
    {
        int score = 0;
        var type = enemy.EnemyType;
        switch (type)
        {
            case EnemyType.Mono:
                {
                    score = -100 - IsHealthLowScore();
                    break;
                }
            case EnemyType.Any:
                {
                    score = IsHealthLowScore();
                    break;
                }
            case EnemyType.Team:
                {
                    score = enemiesAttackingTarget * 100 - IsHealthLowScore();
                    break;
                }
        }
        return score;
    }

    private int IsHealthLowScore() => Observation.Defence.CurrentHealth < Observation.Defence.Health / 2f ? 100 : 0;

}
