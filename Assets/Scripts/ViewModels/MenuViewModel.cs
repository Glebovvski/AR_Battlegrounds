using System;
using Zenject;

public class MenuViewModel
{
    private DefensesViewModel DefensesViewModel { get; set; }

    public bool IsMenuOpen { get; private set; } = true;

    [Inject]
    private void Construct(DefensesViewModel defensesViewModel)
    {
        DefensesViewModel = defensesViewModel;
    }

    public void Play()
    {

    }

    public event Action OnOpen;
    public void OpenMenu()
    {
        IsMenuOpen = true;
        DefensesViewModel.Close();
        OnOpen?.Invoke();
    }

    public event Action OnClose;
    public void Close()
    {
        IsMenuOpen = false;
        DefensesViewModel.Open();
        OnClose?.Invoke();
    }
}
