using CodeBase.DialogsSystem;
using CodeBase.DialogsSystem.LooseDialog;
using CodeBase.DialogsSystem.PauseDialog;
using CodeBase.DialogsSystem.PauseRestartConfirmationDialog;
using CodeBase.DialogsSystem.ReviveHealth;
using CodeBase.DialogsSystem.ReviveMovesDialog;
using CodeBase.DialogsSystem.WinDialog;
using Zenject;

namespace CodeBase.MonoInstallers
{
    public class DialogsInstaller : MonoInstaller<DialogsInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<DialogsController>().FromNew().AsSingle();
            Container.Bind<WinDialog>().FromComponentInHierarchy().AsSingle();
            Container.Bind<LooseDialog>().FromComponentInHierarchy().AsSingle();
            Container.Bind<ReviveMovesDialog>().FromComponentInHierarchy().AsSingle();
            Container.Bind<ReviveHealthDialog>().FromComponentInHierarchy().AsSingle();
            Container.Bind<PauseRestartConfirmationDialog>().FromComponentInHierarchy().AsSingle();
            Container.Bind<PauseDialog>().FromComponentInHierarchy().AsSingle();
        }
    }
}