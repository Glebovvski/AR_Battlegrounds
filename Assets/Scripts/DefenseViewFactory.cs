using Defendable;
using UnityEngine;

public static class DefenseViewFactory
{
    public static DefenseView CreateDefenseView(ScriptableDefense defense, DefenseView prefab, Transform parent, DefensesModel model)
    {
        var view = GameObject.Instantiate(prefab, parent);
        DefenseViewModel vm = new DefenseViewModel(defense, view);
        view.Init(defense, model);
        return view;
    }
}
