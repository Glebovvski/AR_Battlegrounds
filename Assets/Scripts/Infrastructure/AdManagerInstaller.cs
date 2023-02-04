using UnityEngine;
using Zenject;

public class AdManagerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<AdManager>().FromNew().AsSingle().NonLazy();
    }
}