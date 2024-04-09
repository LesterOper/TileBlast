using DG.Tweening;
using UnityEngine;

namespace CodeBase.Extentions.AnimationsExtentions
{
    public static class DOTweenScalingExtentions
    {
        public static Tween Bouncing(this Transform transform, float scaleMod, float duration)
        {
            var scale = transform.localScale;
            return transform.DOScale(scale * scaleMod, duration).OnComplete(() =>
            {
                transform.DOScale(scale, duration);
            });
        }
    }
}