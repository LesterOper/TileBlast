using CodeBase.CoreBase.Configs;
using CodeBase.CoreBase.FieldBase;
using CodeBase.Factory;
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

        public override void InstallBindings()
        {
            Container.Bind<ItemsConfig>().FromScriptableObject(_itemsConfig).AsCached();
            Container.Bind<FieldSettings>().FromScriptableObject(_fieldSettings).AsSingle().WhenInjectedInto<FieldBoard>();
            Container.Bind<LevelsConfig>().FromScriptableObject(_levelsConfig).AsSingle()
                .WhenInjectedInto<InitializeLevelState>();
            Container.Bind<GlobalGameSettingsConfig>().FromScriptableObject(_gameSettingsConfig).AsCached();
        }
    }
}