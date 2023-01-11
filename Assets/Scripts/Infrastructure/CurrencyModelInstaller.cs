using Zenject;

public class CurrencyModelInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        CurrencyModel currencyModel = new CurrencyModel();
        Container.Bind<CurrencyModel>().FromInstance(currencyModel).AsSingle();
    }
}