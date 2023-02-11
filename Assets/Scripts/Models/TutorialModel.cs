using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialModel
{
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

    public event Action<string, int> OnStepInited;

    public void InitStepOne() => OnStepInited?.Invoke("Move your phone around until the plane appears", 1);
    public void InitStepTwo() => OnStepInited?.Invoke("Tap on plane to create Grid (Move your device closer to plane if nothing happens)", 2);
    public void InitStepTwo() => OnStepInited?.Invoke("To set your defense select Tower from the list and put it on available cell", 1);
    public void InitStepThree() => OnStepInited?.Invoke("Your current money count is displayed here", 2);
    public void InitStepFour() => OnStepInited?.Invoke("You can get more money by destroying enemies that come.\nAlso money generate over time with this rate", 3);
    public void InitStepFive() => OnStepInited?.Invoke("When you done start the game by clicking cross", 4);
    public void CompleteTutorial() => IsTutorialCompleted = true;
}
