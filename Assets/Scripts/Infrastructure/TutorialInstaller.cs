using UnityEngine;
using Zenject;

public class TutorialInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<TutorialViewModel>().FromNew().AsSingle();
    }
}