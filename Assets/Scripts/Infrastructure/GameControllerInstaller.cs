using UnityEngine;
using Zenject;

public class GameControllerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<GameControlModel>().FromNew().AsSingle();
    }
}