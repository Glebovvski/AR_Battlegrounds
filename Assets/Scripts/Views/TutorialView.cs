using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DebugView : MonoBehaviour
{
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

    public event Action OnTutorialClick;

    private void Update()
    {
        if (IsClickedOnTutorial)
            OnTutorialClick?.Invoke();
    }
}
