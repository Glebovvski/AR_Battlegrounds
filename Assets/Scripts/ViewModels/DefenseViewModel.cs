using Zenject;

public class DefenseViewModel
{
    private CurrencyModel CurrencyModel { get; set; }

    private DefenseView DefenseView { get; set; }
    private ScriptableDefense Defense { get; set; }

    public DefenseViewModel(ScriptableDefense defense, DefenseView view, CurrencyModel currencyModel)
    {
        CurrencyModel = currencyModel;
        Defense = defense;
        DefenseView = view;
        CurrencyModel.OnGoldAmountChanged += UpdateDefenseAffordable;
    }

    private void UpdateDefenseAffordable()
    {
        DefenseView.UpdateButton(Defense.Price <= CurrencyModel.Gold);
    }
}
