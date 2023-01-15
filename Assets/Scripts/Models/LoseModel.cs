using System.Collections;
using System.Collections.Generic;
using Defendable;
using UnityEngine;
using Zenject;

public class LoseModel : MonoBehaviour
{
    private CastleDefense Castle { get; set; }

    [Inject]
    private void Construct(CastleDefense castle)
    {
        Castle = castle;
    }

    private void Start()
    {
        // Castle.OnDeath +=
    }
}
