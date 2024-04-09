using CodeBase.CoreBase.GameStateModel;
using Zenject;

namespace CodeBase.StateMachine.GameLoopStates
{
    public class LevelProgressCheckerState : State
    {
        private GameEndingChecker _gameEndingChecker;

        public LevelProgressCheckerState(
            SignalBus signalBus,
            GameEndingChecker gameEndingChecker) : base(signalBus)
        {
            _gameEndingChecker = gameEndingChecker;
        }

        public override void Execute()
        {
            base.Execute();
            _gameEndingChecker.CheckGameEnding();
            _signalBus.Fire(new NextStateSignal(typeof(UserClickState)));
        }
    }
}