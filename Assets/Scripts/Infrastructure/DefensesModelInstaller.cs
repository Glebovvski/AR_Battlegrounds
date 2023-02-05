using Defendable;
using Zenject;

public class DefensesModelInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<DefensesModel>().FromNew().AsSingle();
        Container.BindFactory<DefenseView, DefenseViewFactory>().AsSingle();
    }
}