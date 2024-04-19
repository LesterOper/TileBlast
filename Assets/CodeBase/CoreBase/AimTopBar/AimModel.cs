using System.Collections.Generic;
using CodeBase.CoreBase.Configs;

namespace CodeBase.CoreBase.AimTopBar
{
    public interface IAimModelSetter
    {
        List<IAimSetter> Aims { get; }

        void SetLevelAims(List<LevelAim> levelAims);
        bool IsLevelPassed();
    }
    
    public class AimModel : IAimModelSetter
    {
        private List<IAimSetter> _levelAims = new();

        public void SetLevelAims(List<LevelAim> levelAims)
        {
            foreach (var levelAim in levelAims)
                _levelAims.Add(new Aim(levelAim.ItemType, levelAim.amount));
        }
        
        public List<IAimSetter> Aims => _levelAims;

        public bool IsLevelPassed()
        {
            return _levelAims.FindAll(aim => aim.IsComplete).Count == _levelAims.Count;
        }
    }
}