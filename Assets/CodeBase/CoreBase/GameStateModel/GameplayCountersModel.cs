using CodeBase.CoreBase.AimTopBar;

namespace CodeBase.CoreBase.GameStateModel
{
    public class GameplayCountersModel
    {
        private IMovesSetter _movesSetter;
        private IAimModelSetter _aimModelSetter;

        public GameplayCountersModel(IMovesSetter movesSetter, IAimModelSetter aimModelSetter)
        {
            _movesSetter = movesSetter;
            _aimModelSetter = aimModelSetter;
        }

        public IMovesSetter MovesModelProperty => _movesSetter;
        public IAimModelSetter AimModelSetterProperty => _aimModelSetter;
    }
}