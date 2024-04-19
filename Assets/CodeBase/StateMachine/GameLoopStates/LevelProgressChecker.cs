using CodeBase.CoreBase.GameStateModel;
using Zenject;

namespace CodeBase.StateMachine.GameLoopStates
{
    public class LevelProgressChecker : State
    {
        private GameEndingChecker _gameEndingChecker;
        private GameModel _gameModel;
        public LevelProgressChecker(SignalBus signalBus,
            GameModel gameModel,
            GameEndingChecker gameEndingChecker) : base(signalBus)
        {
            _gameEndingChecker = gameEndingChecker;
            _gameModel = gameModel;
        }

        public override void Execute()
        {
            _gameEndingChecker.CheckGameEnding();
            _signalBus.Fire(new NextStateSignal(typeof(UserClickState)));
        }
    }
}