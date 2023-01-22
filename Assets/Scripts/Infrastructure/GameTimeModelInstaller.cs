using UnityEngine;
using Zenject;

public class GameTimeModelInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<GameTimeModel>().FromNew().AsSingle();
    }
}