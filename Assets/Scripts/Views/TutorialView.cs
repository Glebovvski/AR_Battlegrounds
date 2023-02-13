using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Zenject;

public class TutorialView : MonoBehaviour
{
    private TutorialViewModel ViewModel { get; set; }

    [SerializeField] private PositionTextDictionary texts;

    private bool IsClickedOnTutorial => Input.GetMouseButtonDown(0) || IsTouched;

    private bool IsTouched
    {
        get
        {
            if (Input.touchCount == 0) return false;
            var touch = Input.GetTouch(0);
            return touch.phase == TouchPhase.Began;
        }
    }

    [Inject]
    private void Construct(TutorialViewModel tutorialViewModel)
    {
        ViewModel = tutorialViewModel;
    }

    private void Start()
    {
        ViewModel.OnTutorialStart += Open;
        ViewModel.OnStepSet += StartTutorialStep;
        ViewModel.OnTutorialEnd += Close;
    }

    private void StartTutorialStep(string text, TutorialPlacement placement)
    {
        CloseAllTexts();

        var textField = texts[placement];
        textField.SetText(text);
        textField.Show();
    }

    private void CloseAllTexts()
    {
        foreach (var text in texts)
        {
            text.Value.Hide();
        }
    }

    private void Open()
    {
        this.gameObject.SetActive(true);
    }

    private void Close()
    {
        this.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (IsClickedOnTutorial)
        {
            ViewModel.TutorialClick();
        }
    }
}

[Serializable]
public class PositionTextDictionary : SerializableDictionary<TutorialPlacement, TutorialTextComponent>
{

}

[Serializable]
public enum TutorialPlacement
{
    UpperLeft = 0,
    LowerCentre = 1,
    UpperCentre = 2,
    UpperRight = 3,
}