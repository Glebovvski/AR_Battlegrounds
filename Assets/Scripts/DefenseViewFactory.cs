using Defendable;
using UnityEngine;
using Zenject;

public class DefenseViewFactory : PlaceholderFactory<DefenseView>
{
    private CurrencyModel CurrencyModel { get; set; }
    private DiContainer DiContainer { get; set; }

    [Inject]
    private void Construct(CurrencyModel currencyModel, DiContainer container)
    {
        DiContainer = container;
        CurrencyModel = currencyModel;
    }

    public DefenseView CreateDefenseView(ScriptableDefense defense, DefenseView prefab, Transform parent)
    {
        if (defense.Price == 0) return null;

        var view = GameObject.Instantiate(prefab, parent);
        DefenseViewModel vm = DiContainer.Resolve<DefenseViewModel>();// new DefenseViewModel(defense, view, CurrencyModel);
        view.Init(defense, vm);
        return view;
    }
}
