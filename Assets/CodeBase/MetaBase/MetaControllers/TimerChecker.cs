using System;
using CodeBase.ApplicationBase;
using CodeBase.CoreBase;

namespace CodeBase.MetaBase.MetaControllers
{
    public class TimerChecker
    {
        private GlobalGameStateModel _gameStateModel;
        private TimeModel _timeModel;

        public TimerChecker(GlobalGameStateModel gameStateModel, TimeModel timeModel)
        {
            _gameStateModel = gameStateModel;
            _timeModel = timeModel;
        }

        public (long, bool) TryToRecoverHealth()
        {
            long currentTime = _timeModel.GetUnixTimeSeconds();
            long duration = 0;
            bool needRecover = false;
            if (_gameStateModel.IsNeedToRecoverHealth())
            {
                for (int i = 0; i < _gameStateModel.HealthRecovery.Count; i++)
                {
                    var remainingTime = currentTime - _gameStateModel.HealthRecovery[i].EndTime;

                    if (remainingTime <= 0)
                    {
                        duration = Math.Abs(remainingTime);
                        needRecover = true;
                        break;
                    }

                    _gameStateModel.HealthRecovery[i].EndTime = 0;
                    _gameStateModel.HealthRecovery[i].NeedToRecover = false;
                }

                int healthRecovered = _gameStateModel.HealthRecovery.RemoveAll(local => !local.NeedToRecover);
                _gameStateModel.HealthSetter.Value = healthRecovered;
            }
            return (duration, needRecover);
        }
    }
}