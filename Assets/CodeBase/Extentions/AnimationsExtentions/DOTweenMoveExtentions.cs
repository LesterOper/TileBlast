using DG.Tweening;
using UnityEngine;

namespace CodeBase.Extentions.AnimationsExtentions
{
    public static class DOTweenMoveExtentions
    {
        public static Tween MoveLocalToZeroOfParent(this Transform transform, float duration)
        {
            return transform.DOLocalMove(Vector3.zero, duration);
        }
        
        public static Tween MoveLocalToZeroOfParent(this Transform transform, Vector2 target, float duration)
        {
            return transform.DOMove(target, duration);
        }
    }
}