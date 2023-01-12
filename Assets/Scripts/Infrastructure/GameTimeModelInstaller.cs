using UnityEngine;
using Zenject;

public class GameTimeModelInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IInitializable>().To<GameTimeModel>().AsSingle();
    }
}