using Defendable;
using Zenject;

public class DefenseViewModel
{
    private CurrencyModel CurrencyModel { get; set; }

    private DefenseView DefenseView { get; set; }
    private ScriptableDefense Defense { get; set; }

    [Inject]
    private void Construct(DefensesModel defensesModel)
    {
        DefensesModel = defensesModel;
    }

    public DefenseViewModel(ScriptableDefense defense, DefenseView view, CurrencyModel currencyModel)
    {
        CurrencyModel = currencyModel;
        Defense = defense;
        DefenseView = view;
        CurrencyModel.OnGoldAmountChanged += UpdateDefenseAffordable;
    }

    private void UpdateDefenseAffordable(int gold)
    {
        DefenseView.UpdateButton(Defense.Price <= gold);
    }

    public void SelectDefence()
    {
        DefensesModel.DefenseSelected(Defense);
    }
}
