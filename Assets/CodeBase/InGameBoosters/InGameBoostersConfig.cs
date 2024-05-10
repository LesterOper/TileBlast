using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CodeBase.InGameBoosters
{
    [CreateAssetMenu(menuName = "Configs/" + nameof(InGameBoostersConfig), fileName = nameof(InGameBoostersConfig))]
    public class InGameBoostersConfig : ScriptableObject
    {
        public List<InGameBoosterData> inGameBoosterDatas;

        public Sprite Get(InGameBoosterType inGameBoosterData) =>
            inGameBoosterDatas.FirstOrDefault(local => local.inGameBoosterType == inGameBoosterData)?.icon;
    }

    [Serializable]
    public class InGameBoosterData
    {
        public InGameBoosterType inGameBoosterType;
        public Sprite icon;
    }

    public enum InGameBoosterType
    {
        Cocos,
        Hammer,
        Wave,
        Shuffle
    }
}