using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class StatManager : IInitializable
{
    private int enemiesKilled = 0;
    private int defenseDestroyed = 0;

    public int EnemiesKiiled { get => enemiesKilled; set => enemiesKilled = value; }
    public int DefensesDestroyed { get => defenseDestroyed; set => defenseDestroyed = value; }

    public void Initialize()
    {
        Reset();
    }

    public void Reset()
    {
        enemiesKilled = 0;
        defenseDestroyed = 0;
    }
}
