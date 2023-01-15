using System;
using System.Collections;
using System.Collections.Generic;
using Defendable;
using UnityEngine;
using Zenject;

public class LoseViewModel : MonoBehaviour
{
    private CastleDefense Castle { get; set; }

    [SerializeField] LoseView view;

    [Inject]
    private void Construct(CastleDefense castle)
    {
        Castle = castle;
    }

    private void Start()
    {
        Castle.OnDeath += ShowLoseView;
    }

    private void ShowLoseView()
    {
        view.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        Castle.OnDeath -= ShowLoseView;
    }

}
