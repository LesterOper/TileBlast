using CodeBase.CoreBase.GameStateModel;
using Zenject;

namespace CodeBase.CoreBase.Counter
{
    public class MovesCounter : CounterBase
    {
        [Inject]
        private void Construct(IMovesSetter movesSetter) => _counterSetter = movesSetter;
    }
}