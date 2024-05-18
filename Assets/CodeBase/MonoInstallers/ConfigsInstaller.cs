using CodeBase.CoreBase.Configs;
using CodeBase.CoreBase.Configs.LevelSystem;
using CodeBase.CoreBase.FieldBase;
using CodeBase.DialogsSystem.Shop;
using CodeBase.Factory;
using CodeBase.InGameBoosters;
using CodeBase.MetaBase.EventsInGameSystem;
using CodeBase.MetaBase.OfferSystem;
using CodeBase.StateMachine.GameLoopStates;
using UnityEngine;
using Zenject;

namespace CodeBase.MonoInstallers
{
    [CreateAssetMenu(menuName = "Installers/" + nameof(ConfigsInstaller), fileName = nameof(ConfigsInstaller))]

    public class ConfigsInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private ItemsConfig _itemsConfig;
        [SerializeField] private FieldSettings _fieldSettings;
        [SerializeField] private LevelsConfig _levelsConfig;
        [SerializeField] private GlobalGameSettingsConfig _gameSettingsConfig;
        [SerializeField] private VfxConfig _vfxConfig;
        [SerializeField] private ShopConfig _shopConfig;
        [SerializeField] private InGameBoostersConfig _inGameBoostersConfig;
        [SerializeField] private EventsInGameConfig _eventsInGameConfig;
        [SerializeField] private OffersInGameConfig _offersInGameConfig;

        public override void InstallBindings()
        {
            Container.Bind<VfxConfig>().FromScriptableObject(_vfxConfig).AsSingle();
            Container.Bind<ItemsConfig>().FromScriptableObject(_itemsConfig).AsCached();
            Container.Bind<FieldSettings>().FromScriptableObject(_fieldSettings).AsSingle().WhenInjectedInto<FieldBoard>();
            Container.Bind<LevelsConfig>().FromScriptableObject(_levelsConfig).AsSingle();
            Container.Bind<GlobalGameSettingsConfig>().FromScriptableObject(_gameSettingsConfig).AsCached();
            Container.Bind<ShopConfig>().FromScriptableObject(_shopConfig).AsCached();
            Container.Bind<InGameBoostersConfig>().FromScriptableObject(_inGameBoostersConfig).AsCached();
            Container.Bind<EventsInGameConfig>().FromScriptableObject(_eventsInGameConfig).AsCached();
            Container.Bind<OffersInGameConfig>().FromScriptableObject(_offersInGameConfig).AsCached();
        }
    }
}