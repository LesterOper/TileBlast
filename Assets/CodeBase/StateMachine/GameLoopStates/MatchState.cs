using CodeBase.ApplicationBase.AudioManager;
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

        public MatchState(
            SignalBus signalBus,
            Matcher matcher,
            GameModel gameModel) : base(signalBus)
        {
            _matcher = matcher;
            _gameModel = gameModel;
        }

        public override void Execute()
        {
            base.Execute();
            if (!_gameModel.IsClickedFieldPositionOutOfIndex())
            {
                AudioManager.Inst.PlayBlocksTaps();
                _matcher.TryToMatch(_gameModel.ClickFieldPosition, () => _signalBus.Fire(new NextStateSignal(typeof(MoveState))));
            }
            else
            {
                _signalBus.Fire(new NextStateSignal(typeof(UserClickState)));
            }
        }
    }
}