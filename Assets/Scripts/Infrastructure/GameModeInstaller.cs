using Zenject;

public class GameModeInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<GameModeModel>().FromNew().AsSingle();
    }
}