using Sirenix.OdinInspector;
using UnityEngine;

namespace CodeBase.CoreBase.FieldBase
{
    [CreateAssetMenu(fileName = nameof(FieldSettings), menuName = "Settings/" + nameof(FieldSettings))]
    public sealed class FieldSettings : ScriptableObject
    {
        public float tileOriginalSize;

        [BoxGroup]
        [InfoBox("Top border")] [Range(0, 1)] public float top;
        [BoxGroup]
        [InfoBox("Bottom border")] [Range(0, 1)] public float bottom;
        [BoxGroup]
        [InfoBox("Edge border")] [Range(0, 1)] public float edge;
        [Range(0, 10)]
        public float borderDelta;
    }
}