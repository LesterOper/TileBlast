using System.Collections.Generic;
using System.Linq;
using CodeBase.ApplicationBase;
using CodeBase.CoreBase.CellBase;
using CodeBase.CoreBase.Configs;
using CodeBase.CoreBase.Configs.LevelSystem;
using CodeBase.CoreBase.FieldBase;
using CodeBase.CoreBase.GameStateModel;
using CodeBase.CoreBase.Item;
using CodeBase.Factory;
using CodeBase.StateMachine;
using CodeBase.StateMachine.GameLoopStates;
using UnityEngine;
using Zenject;

namespace CodeBase.CoreBase.AimTopBar
{
    public class AimController
    {
        private GameModel _gameModel;
        private SpawnController _spawnController;
        private SignalBus _signalBus;
        private FieldBoard _fieldBoard;

        public AimController(GameModel gameModel, SpawnController spawnController, SignalBus signalBus, FieldBoard fieldBoard)
        {
            _gameModel = gameModel;
            _spawnController = spawnController;
            _signalBus = signalBus;
            _fieldBoard = fieldBoard;
        }

        public void UpdateAims(ItemType itemType = ItemType.NONE, int amount = 0, FieldPosition fieldPosition = null)
        {
            LevelType levelType = _gameModel.Level.levelData.levelType;

            switch (levelType)
            {
                case LevelType.COLLECT_OBJECTS:
                {
                    CollectObjects(itemType, amount, fieldPosition);
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

            if (aimSetter is {IsComplete: false})
            {
                List<IFieldCell> empty = _gameModel.FieldModel.GetEmptyCells();
                for (int i = 0; i < amount; i++)
                {
                    ItemView itemView = _spawnController.SpawnAnimatedItemView(
                        _fieldBoard.WorldPosition(empty[i].FieldPositionProperty.Row,
                            empty[i].FieldPositionProperty.Column), itemType);
                    AnimationSource.ParabollaForExist(aimSetter.counterTr, itemView.R, 0.65f, () =>
                    {
                        aimSetter.Value = - 1;
                    });
                }
            }
        }

        private void CollectObjects(ItemType itemType, int amount, FieldPosition fieldPosition)
        {
            IAimSetter aimSetter =
                _gameModel.GameplayCountersModelProperty.AimModelSetterProperty.Aims.FirstOrDefault(local =>
                    local.AimItemType == itemType);

            if (aimSetter is {IsComplete: false})
            {
                ItemView itemView =
                    _spawnController.SpawnAnimatedItemView(_fieldBoard.WorldPosition(fieldPosition), itemType);

                void AnimCallback()
                {
                    aimSetter.Value = -amount;
                }
                AnimationSource.ParabollaForExist(aimSetter.counterTr, itemView.R, 0.65f, AnimCallback);
            }
        }

        private void ClearField()
        {
            IAimSetter aimSetter = _gameModel.GameplayCountersModelProperty.AimModelSetterProperty.Aims[0];

            aimSetter.Value = -(aimSetter.Value -
                                (_gameModel.Level.levelData.LevelAims[0].amount - _gameModel.FieldModel.GetEmptyCellsCount()));
        }

        private void DropObjectsToBottom()
        {
            bool collect = false;
            IAimSetter aimSetter = _gameModel.GameplayCountersModelProperty.AimModelSetterProperty.Aims[0];

            var fieldCells = _gameModel.FieldModel.GetFirstActiveCellsInColumnWithDropDownItems();

            for (int i = 0; i < fieldCells.Count; i++)
            {
                IFieldCell cellBase = fieldCells[i];
                if (fieldCells[i].IsDropDown)
                {
                    SpriteRenderer animate = cellBase.Cell.CellViewProperty.R;
                    AnimationSource.Parabolla(aimSetter.counterTr, animate, 1f, ()=>{aimSetter.Value = -1;});
                    _spawnController.Despawn(cellBase.Cell.CellViewProperty);
                    cellBase.Cell = new EmptyCell();
                    collect = true;
                }
            }
            
            if(collect)
                _signalBus.Fire(new NextStateSignal(typeof(UserClickState)));
        }
    }
}