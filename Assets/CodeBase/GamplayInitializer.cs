using CodeBase.StateMachine;
using Zenject;

namespace CodeBase
{
    public class GamplayInitializer : IInitializable
    {
        private GameStateMachine _gameStateMachine;

        public GamplayInitializer(GameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }
        
        public void Initialize()
        {
            _gameStateMachine.RunGameLoop();
        }
    }
}