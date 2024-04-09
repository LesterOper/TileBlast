using CodeBase.CoreBase.CellBase;
using CodeBase.CoreBase.Item;
using CodeBase.Factory;
using UnityEngine;
using Zenject;

namespace CodeBase.MonoInstallers
{
    public class PrefabsInstaller : MonoInstaller<PrefabsInstaller>
    {
        [SerializeField] private ItemView _itemView;
        [SerializeField] private CellView _cellView;
        
        public override void InstallBindings()
        {
            Container.Bind<ItemView>().FromComponentOn(_itemView.gameObject).AsCached();
            Container.Bind<CellView>().FromComponentOn(_cellView.gameObject).AsCached().WhenInjectedInto<CellFactory>();
        }
    }
}