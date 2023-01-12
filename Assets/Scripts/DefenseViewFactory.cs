using Defendable;
using UnityEngine;
using Zenject;

public class DefenseViewFactory : PlaceholderFactory<DefenseView>
{
    private CurrencyModel CurrencyModel { get; set; }
    private DefensesModel DefensesModel { get; set; }


    [Inject]
    private void Construct(CurrencyModel currencyModel, DefensesModel defensesModel)
    {
        CurrencyModel = currencyModel;
        DefensesModel = defensesModel;
    }

    public DefenseView CreateDefenseView(ScriptableDefense defense, DefenseView prefab, Transform parent)
    {
        if (defense.Price == 0) return null;

        var view = GameObject.Instantiate(prefab, parent);
        DefenseViewModel vm = new DefenseViewModel(defense, view, CurrencyModel, DefensesModel);
        view.Init(defense, vm);
        return view;
    }
}
