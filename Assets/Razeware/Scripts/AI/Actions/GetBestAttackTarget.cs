using System.Collections.Generic;
using System.Linq;
using AI;
using Apex.AI;
using Apex.Serialization;
using Enemies;
using UnityEngine;
using UnityEngine.AI;

public class GetBestAttackTarget : ActionBase
{
    [ApexSerialization] private int MaxEnemiesToAttackOneTarget = 3;
    List<TargetScore> scores = new List<TargetScore>();
    public override void Execute(IAIContext context)
    {
        var c = (AIContext)context;
        var enemy = c.Enemy;

        if (enemy.GetAttackTarget() != null) return;

        foreach (var defense in enemy.DefenseTypeToScore)
        {
            var bestDefenseType = defense;
            var closestObservation = Enemies.AIManager.Instance.GetClosestByType(enemy, defense.Key);
            if (closestObservation == null)
                continue;
            // closestObservation.SetPosition(closestObservation.Defense.transform.right * -2f);
            var path = enemy.GetCalculatedPath(closestObservation);
            if (path.status == NavMeshPathStatus.PathInvalid)
                continue;
            var enemiesWithSameTarget = Enemies.AIManager.Instance.GetEnemiesAttackingObservation(closestObservation);
            scores.Add(new TargetScore(path.corners.Length, enemiesWithSameTarget, enemy, closestObservation, defense.Value, MaxEnemiesToAttackOneTarget));
        }
        var attackTarget = scores.OrderByDescending(x => x.TotalScore).First().Observation;
        Debug.LogError(string.Format("ENEMY: {0} TARGET: {1}", enemy.name, attackTarget.Defense.gameObject.name));
        c.Enemy.SetAttackTarget(attackTarget);
    }
}

public class TargetScore
{
    private int pathLengthScore;
    private int enemiesAttackingTarget;
    private Enemy enemy;
    private int extraScore;
    private int maxEnemies;
    public Observation Observation { get; private set; }

    public int TotalScore => GetTotalScore();

    public TargetScore(int pathLength, int enemiesAttackingTarget, Enemy enemy, Observation observation, int extraScore, int maxEnemies)
    {
        pathLengthScore = 100 - pathLength;
        this.enemiesAttackingTarget = enemiesAttackingTarget;
        this.enemy = enemy;
        Observation = observation;
        this.extraScore = extraScore;
        this.maxEnemies = maxEnemies;
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
                    // Debug.LogError("ENEMIES ATTACKING TARGET: "+enemiesAttackingTarget);
                    var enemiesAttackingTargetScore = enemiesAttackingTarget <= maxEnemies ? enemiesAttackingTarget : -extraScore;
                    score = enemiesAttackingTargetScore * 100 - IsHealthLowScore();
                    break;
                }
        }
        return score;
    }

    private int IsHealthLowScore() => Observation.Defense.CurrentHealth < Observation.Defense.Health / 2f ? 100 : 0;

}
