using CodeBase.ApplicationBase;
using CodeBase.ApplicationBase.SavingBase;
using CodeBase.CoreBase;
using CodeBase.DialogsSystem.Shop;
using CodeBase.Extentions.Time;
using Zenject;

namespace CodeBase
{
    public class GlobalInstaller : MonoInstaller<GlobalInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<Initializer>().FromNew().AsSingle();
            Container.BindInterfacesTo<CurrencyModel>().FromNew().AsCached();
            Container.BindInterfacesTo<HealthModel>().FromNew().AsSingle();
            Container.Bind<GlobalGameStateModel>().FromNew().AsSingle();
            Container.Bind<CommonSaving>().FromNew().AsSingle();
            Container.Bind<TimeModel>().FromNew().AsSingle();
            Container.Bind<GetTimeFromTimeAPI>().FromNew().AsSingle();
            Container.Bind<CoroutineRunner>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<InAppPurchasingManager>().FromNew().AsSingle();
        }
    }
}