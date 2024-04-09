using CodeBase.CoreBase.Configs;
using CodeBase.CoreBase.GameStateModel;
using UnityEngine;

namespace CodeBase.CoreBase.AimTopBar
{
    public class AimTopBarController
    {
        private GameModel _gameModel;
        private ItemsConfig _itemsConfig;

        public AimTopBarController(GameModel gameModel, ItemsConfig itemsConfig)
        {
            _gameModel = gameModel;
            _itemsConfig = itemsConfig;
        }

        public void InitializeLevelAim(AimView aimPrefab, Transform parent)
        {
            IAimModelSetter aimModelSetter = _gameModel.GameplayCountersModelProperty.AimModelSetterProperty;
            for (int i = 0; i < aimModelSetter.Aims.Count; i++)
            {
                IAimSetter aimSetter = aimModelSetter.Aims[i];
                AimView aimView = Object.Instantiate(aimPrefab, parent);
                aimView.Initialize(aimSetter, _itemsConfig.GetItem(aimSetter.AimItemType).item);
                aimModelSetter.Aims[i].OnUpdateCounterEvent += aimView.UpdateAimCounter;
            }
        }
    }
}