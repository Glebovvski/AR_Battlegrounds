using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TutorialModel : IInitializable
{
    private PlaneManager PlaneManager { get; set; }
    private MenuViewModel MenuViewModel { get; set; }

    private const string isTutorialCompletedKey = "IsTutorialCompleted";

    private bool isTutorialCompleted = false;
    public bool IsTutorialCompleted
    {
        get => PlayerPrefs.GetInt(isTutorialCompletedKey, 0) == 1;
        private set
        {
            isTutorialCompleted = value;
            PlayerPrefs.SetInt(isTutorialCompletedKey, isTutorialCompleted ? 1 : 0);
        }
    }

    [Inject]
    private void Construct(PlaneManager planeManager, MenuViewModel menuViewModel)
    {
        PlaneManager = planeManager;
        MenuViewModel = menuViewModel;
    }

    public void Initialize()
    {
        MenuViewModel.OnClose += StartTutorial;
    }

    private void StartTutorial()
    {
        if (IsTutorialCompleted) return;

        InitStepOne();
    }

    public event Action<string> OnStepInited;

    public void InitStepOne() => OnStepInited?.Invoke("Move your phone around until the plane appears");
    public void InitStepTwo() => OnStepInited?.Invoke("Tap on plane to create Grid (Move your device closer to plane if nothing happens)");
    public void InitStepThree() => OnStepInited?.Invoke("To set your defense select Tower from the list and put it on available cell");
    public void InitStepFour() => OnStepInited?.Invoke("Your current money count is displayed here");
    public void InitStepFive() => OnStepInited?.Invoke("You can get more money by destroying enemies that come.\nAlso money generate over time with this rate");
    public void InitStepSix() => OnStepInited?.Invoke("When you done start the game by clicking cross");
    public void CompleteTutorial() => IsTutorialCompleted = true;
}