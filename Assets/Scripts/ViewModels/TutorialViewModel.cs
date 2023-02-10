using System;
using UnityEngine;

public class TutorialViewModel
{
    public event Action OnTutorialStart;
    public void StartTutorial() => OnTutorialStart?.Invoke();
}
