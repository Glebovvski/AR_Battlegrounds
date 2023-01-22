using UnityEngine;
using Zenject;

public class WinModelInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<WinModel>().FromNew().AsSingle();
    }
}