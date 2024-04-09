using System;
using CodeBase.CoreBase;
using CodeBase.CoreBase.FieldBase;
using CodeBase.CoreBase.GameStateModel;
using CodeBase.CoreBase.Input;
using UnityEngine;
using Zenject;

namespace CodeBase.StateMachine.GameLoopStates
{
    public class UserClickState : State
    {
        private IInputSystem _inputSystem;
        private FieldBoard _fieldBoard;
        private GameModel _gameModel;

        public UserClickState(
            SignalBus signalBus,
            IInputSystem inputSystem,
            FieldBoard fieldBoard,
            GameModel gameModel) : base(signalBus)
        {
            _inputSystem = inputSystem;
            _fieldBoard = fieldBoard;
            _gameModel = gameModel;
        }

        public override void Execute()
        {
            base.Execute();
            _inputSystem.OnClick += OnClickHandler;
            _inputSystem.SetEnable(true);
        }

        private void OnClickHandler(Vector2 clickPosition)
        {
            _inputSystem.SetEnable(false);
            CalculateFieldPositionAccordingToClick(clickPosition);
            _signalBus.Fire(new NextStateSignal(typeof(MatchState)));
        }

        public override void Exit()
        {
            base.Exit();
            _inputSystem.OnClick -= OnClickHandler;
        }

        private void CalculateFieldPositionAccordingToClick(Vector2 position)
        {
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(position);
            float x = (worldPosition.x - _fieldBoard.OriginalPosition.x) / _fieldBoard.TileSize;
            float y = (worldPosition.y - _fieldBoard.OriginalPosition.y) / _fieldBoard.TileSize;
            FieldPosition fieldPosition = new FieldPosition(Convert.ToInt32(y), Convert.ToInt32(x));
            _gameModel.ClickFieldPosition = fieldPosition;
        }
    }
}