using CodeBase.DialogsSystem;
using CodeBase.DialogsSystem.BuyLivesDialog;
using CodeBase.DialogsSystem.Settings;
using CodeBase.DialogsSystem.Shop;
using CodeBase.MetaBase.Views;
using Zenject;

namespace CodeBase.MetaBase
{
    public class MetaInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<HealthRecoveryTimerView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<MetaDialogController>().FromNew().AsSingle();
            Container.Bind<BuyMoreLivesDialog>().FromComponentInHierarchy().AsSingle();
            Container.Bind<SettingsDialog>().FromComponentInHierarchy().AsSingle();
            Container.Bind<ShopDialog>().FromComponentInHierarchy().AsSingle();
        }
    }
}