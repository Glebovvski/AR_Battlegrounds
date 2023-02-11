using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TutorialModel : IInitializable
{
    private PlaneManager PlaneManager { get; set; }
    private MenuViewModel MenuViewModel { get; set; }
    private TutorialViewModel TutorialViewModel { get; set; }

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

    private int simpleTutorialStepIndex = 0;
    private Action[] simpleTutorialSteps;

    [Inject]
    private void Construct(PlaneManager planeManager, MenuViewModel menuViewModel, TutorialViewModel tutorialViewModel)
    {
        PlaneManager = planeManager;
        MenuViewModel = menuViewModel;
        TutorialViewModel = tutorialViewModel;
    }

    public void Initialize()
    {
#if !UNITY_EDITOR
        MenuViewModel.OnClose += InitStepOne;
        PlaneManager.OnPlanesChanged += InitStepTwo;
        PlaneManager.OnGridSet += InitStepThree;
#endif
        simpleTutorialStepIndex = 0;
        simpleTutorialSteps = new Action[]
        {
            InitStepFour,
            InitStepFive,
            InitStepSix
        };

        TutorialViewModel.OnTutorialClick += InitSimpleStep;
    }

    public event Action<string, TutorialPlacement> OnStepInited;

    public void InitStepOne()
    {
        if (IsTutorialCompleted) return;
        OnStepInited?.Invoke("Move your phone around until the plane appears", TutorialPlacement.UpperCentre);
    }
    public void InitStepTwo()
    {
        if (IsTutorialCompleted) return;
        OnStepInited?.Invoke("Tap on plane to create Grid (Move your device closer to plane if nothing happens)", TutorialPlacement.UpperCentre);
    }
    public void InitStepThree()
    {
        if (IsTutorialCompleted) return;
        OnStepInited?.Invoke("To set your defense select Tower from the list and put it on available cell", TutorialPlacement.LowerCentre);
    }
    public void InitStepFour()
    {
        if (IsTutorialCompleted) return;
        OnStepInited?.Invoke("Your current money count is displayed here", TutorialPlacement.UpperLeft);
    }
    public void InitStepFive()
    {
        if (IsTutorialCompleted) return;
        OnStepInited?.Invoke("You can get more money by destroying enemies that come.\nAlso money generate over time with this rate", TutorialPlacement.UpperLeft);
    }
    public void InitStepSix()
    {
        if (IsTutorialCompleted) return;
        OnStepInited?.Invoke("When you done start the game by clicking cross", TutorialPlacement.UpperRight);
    }
    public void InitSimpleStep()
    {
        if (IsTutorialCompleted) return;
        if (simpleTutorialStepIndex >= simpleTutorialSteps.Length)
        {
            CompleteTutorial();
            return;
        }
        simpleTutorialSteps[simpleTutorialStepIndex].Invoke();
        simpleTutorialStepIndex++;
    }
    public void CompleteTutorial() => IsTutorialCompleted = true;
}