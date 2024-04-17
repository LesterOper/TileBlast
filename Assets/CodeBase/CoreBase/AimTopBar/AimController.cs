using System.Linq;
using CodeBase.CoreBase.CellBase;
using CodeBase.CoreBase.Configs;
using CodeBase.CoreBase.GameStateModel;
using CodeBase.Factory;
using Zenject;

namespace CodeBase.CoreBase.AimTopBar
{
    public class AimController
    {
        private GameModel _gameModel;
        private SpawnController _spawnController;

        public AimController(GameModel gameModel)
        {
            _gameModel = gameModel;
        }

        public AimController(GameModel gameModel, SpawnController spawnController)
        {
            _gameModel = gameModel;
            _spawnController = spawnController;
        }

        public void UpdateAims(ItemType itemType = ItemType.NONE, int amount = 0)
        {
            LevelType levelType = _gameModel.Level.levelType;

            switch (levelType)
            {
                case LevelType.COLLECT_OBJECTS:
                {
                    CollectObjects(itemType, amount);
                    break;
                }

                case LevelType.CLEAR_FIELD:
                {
                    ClearField();
                    break;
                }

                case LevelType.DROP_OBJECTS_TO_BOTTOM:
                {
                    DropObjectsToBottom();
                    break;
                }
            }
        }

        private void CollectObjects(ItemType itemType, int amount)
        {
            IAimSetter aimSetter =
                _gameModel.GameplayCountersModelProperty.AimModelSetterProperty.Aims.FirstOrDefault(local =>
                    local.AimItemType == itemType);

            if (aimSetter != null)
            {
                aimSetter.Value = -amount;
            }
        }

        private void ClearField()
        {
            IAimSetter aimSetter = _gameModel.GameplayCountersModelProperty.AimModelSetterProperty.Aims[0];

            aimSetter.Value = -(aimSetter.Value -
                              (_gameModel.Level.LevelAims[0].amount - _gameModel.FieldModel.GetEmptyCellsCount()));
        }

        private void DropObjectsToBottom()
        {
            ICell[] firstRow = _gameModel.FieldModel.GetFirstRow();

            IAimSetter aimSetter = _gameModel.GameplayCountersModelProperty.AimModelSetterProperty.Aims[0];
            for (int i = 0; i < firstRow.Length; i++)
            {
                if(firstRow[i].ItemDataProperty == null) continue;
                if (firstRow[i].ItemDataProperty.IsDropBottomItem)
                {
                    aimSetter.Value = -1;
                    _spawnController.Despawn(firstRow[i].CellViewProperty);
                    firstRow[i].ClearCell();
                }
            }
        }
    }
}