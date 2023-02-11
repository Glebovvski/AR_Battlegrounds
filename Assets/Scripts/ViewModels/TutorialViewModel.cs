using System;
using Zenject;

public class TutorialViewModel : IInitializable
{
    private TutorialModel TutorialModel { get; set; }

    [Inject]
    private void Construct(TutorialModel tutorialModel)
    {
        TutorialModel = tutorialModel;
    }

    public void Initialize()
    {
        TutorialModel.OnStepInited += InitTutorialStep;
    }

    public event Action<string, TutorialPlacement> OnStepSet;
    private void InitTutorialStep(string text, TutorialPlacement placement) => OnStepSet?.Invoke(text, placement);

    public event Action OnTutorialStart;
    public void StartTutorial() => OnTutorialStart?.Invoke();

    public event Action OnTutorialClick;
    public void TutorialClick() => OnTutorialClick?.Invoke();
}
