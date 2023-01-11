using Defendable;
using UnityEngine;

public class DefenseViewFactory
{
    public DefenseViewFactory()
    {
        
    }
    
    public DefenseView CreateDefenseView(ScriptableDefense defense, DefenseView prefab, Transform parent, DefensesModel model)
    {
        var view = GameObject.Instantiate(prefab);
        DefenseViewModel vm = new DefenseViewModel(defense, view);
        view.Init(defense, model);
        return view;
    }
}
