using CodeBase.CoreBase.BoostersOnField;
using CodeBase.CoreBase.FieldBase;
using CodeBase.CoreBase.GameStateModel;
using DG.Tweening;
using Zenject;

namespace CodeBase.StateMachine.GameLoopStates
{
    public class MatchState : State
    {
        private Matcher _matcher;
        private GameModel _gameModel;
        private BoostersOnFieldActivator _boostersOnFieldActivator;

        public MatchState(
            SignalBus signalBus,
            Matcher matcher,
            GameModel gameModel,
            BoostersOnFieldActivator boostersOnFieldActivator) : base(signalBus)
        {
            _matcher = matcher;
            _gameModel = gameModel;
            _boostersOnFieldActivator = boostersOnFieldActivator;
        }

        public override void Execute()
        {
            base.Execute();
            if (!_gameModel.IsClickedFieldPositionOutOfIndex())
            {
                if (_gameModel.IsClickedItemBooster())
                {
                    _boostersOnFieldActivator.ActivateFieldBooster(_gameModel.ClickFieldPosition, _gameModel.ClickedItemType());
                }
                else _matcher.TryToMatch(_gameModel.ClickFieldPosition);
                DOVirtual.DelayedCall(0.2f, ()=>_signalBus.Fire(new NextStateSignal(typeof(MoveState))));
            }
            else
            {
                _signalBus.Fire(new NextStateSignal(typeof(UserClickState)));
            }
        }
    }
}