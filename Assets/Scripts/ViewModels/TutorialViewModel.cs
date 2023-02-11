using System;

public class TutorialViewModel
{
    public event Action OnTutorialStart;
    public void StartTutorial() => OnTutorialStart?.Invoke();

    public event Action OnTutorialClick;
    public void TutorialClick() => OnTutorialClick?.Invoke();
}
