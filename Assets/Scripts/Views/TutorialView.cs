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
    // public void SetText(string value) => text.text = value;

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
    }

    private void StartTutorialStep(string text, TutorialPlacement placement)
    {
        CloseAllTexts();

        var textField = texts[placement];
        textField.text = text;
        textField.gameObject.SetActive(true);
    }

    private void CloseAllTexts()
    {
        foreach (var text in texts)
        {
            text.Value.gameObject.SetActive(false);
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

    // public event Action OnTutorialClick;

    private void Update()
    {
        if (IsClickedOnTutorial)
            ViewModel.TutorialClick();
        // OnTutorialClick?.Invoke();
    }
}

[Serializable]
public class PositionTextDictionary : SerializableDictionary<TutorialPlacement, TextMeshProUGUI>
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