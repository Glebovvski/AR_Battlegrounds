using Defendable;
using Zenject;

public class DefensesModelInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        DefensesModel defensesModel = new DefensesModel();
        Container.Bind<DefensesModel>().FromInstance(defensesModel).AsSingle();
        Container.BindFactory<DefenseView, DefenseViewFactory>().AsSingle();
    }
}