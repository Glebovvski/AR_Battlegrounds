using System;

public class TutorialViewModel
{
    public event Action OnTutorialStart;
    public void StartTutorial() => OnTutorialStart?.Invoke();
}
