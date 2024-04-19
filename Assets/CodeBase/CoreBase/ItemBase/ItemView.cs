using System;
using CodeBase.CoreBase.Configs;
using CodeBase.Extentions.AnimationsExtentions;
using DG.Tweening;
using UnityEngine;

namespace CodeBase.CoreBase.Item
{
    public class ItemView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer itemIcon;
        private Tween _moveTween;

        public SpriteRenderer R => itemIcon;
        
        public void Initialize(Sprite itemSprite)
        {
            itemIcon.sprite = itemSprite;
        }

        public void FadeOut(float endValue, float duration) => itemIcon.DOFade(endValue, duration);
        
        public void ShakeAnimation() => transform.Bouncing(1.1f, 0.1f);

        public void MoveToTargetCell(Vector2 position, Action callback)
        {
            _moveTween?.Kill();
            _moveTween = transform.MoveLocalToZeroOfParent(position, 0.4f)
                .SetEase(Ease.OutQuart)
                .OnComplete(callback.Invoke);
        }
    }
}