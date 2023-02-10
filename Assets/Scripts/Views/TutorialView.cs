using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Zenject;

public class DebugView : MonoBehaviour
{
    private TutorialViewModel ViewModel { get; set; }

    [SerializeField] private TextMeshProUGUI text;
    public void SetText(string value) => text.text = value;

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
    }

    private void Open()
    {
        this.gameObject.SetActive(true);
    }

    public event Action OnTutorialClick;

    private void Update()
    {
        if (IsClickedOnTutorial)
            OnTutorialClick?.Invoke();
    }
}
