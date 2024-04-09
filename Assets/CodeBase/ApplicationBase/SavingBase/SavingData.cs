using System.Collections.Generic;
using CodeBase.CoreBase;

namespace CodeBase.ApplicationBase.SavingBase
{
    public class SavingData
    {
        public int _softMoney;
        public int _health;
        public int _currentLevel;

        public List<Recovery> _recoveryHealthData = new(5);

        public SavingData(int softMoney, int health, int currentLevel, List<Recovery> recoveries)
        {
            _softMoney = softMoney;
            _health = health;
            _currentLevel = currentLevel;
            _recoveryHealthData.AddRange(recoveries);
        }

        public string ToString()
        {
            string recover = "";
            if (_recoveryHealthData.Count == 0)
                recover = $"Recovery don't need";
            for (int i = 0; i < _recoveryHealthData.Count; i++)
            {
                recover += $"Index: {i} Time: {_recoveryHealthData[i].EndTime} \n";
            }
            
            return $"Money: {_softMoney}\n Health: {_health}\n CurrentLevel: {_currentLevel}\n Recovery: \n {recover}";
        }
    }
}