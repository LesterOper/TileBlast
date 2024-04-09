using CodeBase.DialogsSystem;
using CodeBase.DialogsSystem.LooseDialog;
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
        }
    }
}