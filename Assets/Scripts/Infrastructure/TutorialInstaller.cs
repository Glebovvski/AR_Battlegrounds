using UnityEngine;
using Zenject;

public class TutorialInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<TutorialViewModel>().FromNew().AsSingle();
        Container.BindInterfacesAndSelfTo<TutorialModel>().FromNew().AsSingle();
    }
}