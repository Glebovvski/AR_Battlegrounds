using System;
using System.Collections;
using System.Collections.Generic;
using Defendable;
using UnityEngine;
using Zenject;

public class LoseViewModel : MonoBehaviour
{
    private CastleDefense Castle { get; set; }
    private GameControlModel GameControlModel { get; set; }

    [SerializeField] private LoseView view;

    [Inject]
    private void Construct(CastleDefense castle, GameControlModel gameModel)
    {
        Castle = castle;
        GameControlModel = gameModel;
    }

    private void Start()
    {
        Castle.OnDeath += ShowLoseView;
        view.OnTryAgainClick += TryAgain;
    }

    private void ShowLoseView()
    {
        view.gameObject.SetActive(true);
    }

    public void TryAgain()
    {
        GameControlModel.Restart();
        view.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        Castle.OnDeath -= ShowLoseView;
    }

}
