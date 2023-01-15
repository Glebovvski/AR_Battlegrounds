using Defendable;
using Zenject;

public class DefensesModelInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<DefensesModel>().FromNew().AsSingle();
        Container.BindFactory<DefenseView, DefenseViewFactory>().AsSingle();
    }
}