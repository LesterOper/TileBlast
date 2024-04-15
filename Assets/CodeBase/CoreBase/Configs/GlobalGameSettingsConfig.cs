using UnityEngine;

namespace CodeBase.CoreBase.Configs
{
    [CreateAssetMenu(menuName = "GlobalGameConfig/" + nameof(GlobalGameSettingsConfig), fileName = nameof(GlobalGameSettingsConfig))]
    public class GlobalGameSettingsConfig : ScriptableObject
    {
        public int HealthAmountOnGameStartedFirstTime;
        public int DefaultSoftMoneyAmount;
        public int MaxHealthAmount;
        public float ResetDurationOfOneHealthInSeconds;

        public int MovesReviveCost;
        public int MovesReviveAmount;

        public int HealthReviveCost;
        public int HealthReviveAmount;
    }
}