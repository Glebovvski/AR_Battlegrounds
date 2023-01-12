using Zenject;

public class CurrencyModelInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        CurrencyModel currencyModel = new CurrencyModel();
        Container.Bind<CurrencyModel>().FromNew().AsSingle();//.FromInstance(currencyModel).AsSingle();
        
        GoldViewModel goldViewModel = new GoldViewModel(currencyModel);
        Container.Bind<GoldViewModel>().FromNew().AsSingle();//.FromInstance(goldViewModel).AsSingle();
    }
}