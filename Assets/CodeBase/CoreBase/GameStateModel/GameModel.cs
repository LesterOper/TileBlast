using System;
using CodeBase.CoreBase.CellBase;
using CodeBase.CoreBase.Configs;
using CodeBase.CoreBase.FieldBase;
using CodeBase.DialogsSystem;

namespace CodeBase.CoreBase.GameStateModel
{
    public class GameModel
    {
        private GameplayCountersModel _gameplayCountersModel;
        private DialogsController _dialogsController;
        private FieldModel _fieldModel;

        public FieldPosition ClickFieldPosition { get; set; }
        public Level Level { get; set; }

        public GameModel(
            GameplayCountersModel gameplayCountersModel,
            DialogsController dialogsController,
            FieldModel fieldModel)
        {
            _gameplayCountersModel = gameplayCountersModel;
            _dialogsController = dialogsController;
            _fieldModel = fieldModel;
        }

        public GameplayCountersModel GameplayCountersModelProperty => _gameplayCountersModel;
        public DialogsController DialogsController => _dialogsController;
        public FieldModel FieldModel => _fieldModel;

        public bool IsClickedFieldPositionOutOfIndex() => _fieldModel.IsFieldPositionOutOfSize(ClickFieldPosition);

        public bool IsClickedItemBooster() => _fieldModel.IsClickedItemBooster(ClickFieldPosition);

        public ItemType ClickedItemType() => _fieldModel.GetItemTypeByFieldPosition(ClickFieldPosition);
    }
}