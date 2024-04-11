using System;
using CodeBase.CoreBase.Item;
using CodeBase.Extentions.AnimationsExtentions;
using DG.Tweening;
using UnityEngine;

namespace CodeBase.CoreBase.CellBase
{
    public class CellView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _renderComponent;
        [SerializeField] private ItemView _itemView;
        [SerializeField] private ParticleSystem _particleSystem;
        
        private Tween _moveTween;

        public Transform CellTransform => transform;

        public void Initialize(Sprite coloredCellSprite, Sprite itemSprite)
        {
            _renderComponent.sprite = coloredCellSprite;
            _itemView.Initialize(itemSprite);
        }

        public virtual void FadeCell(float endValue, float duration)
        {
            _renderComponent.DOFade(endValue, duration);
            _itemView.FadeOut(endValue, duration);
        }
        
        public void PlayEffect(Action callback)
        {
            FadeCell(0, 0.1f);
            _particleSystem.Play();
            DOVirtual.DelayedCall(_particleSystem.main.duration, callback.Invoke);
        }
        
        public void MoveToTargetCell(Vector2 position, Action callback)
        {
            _moveTween?.Kill();
            _moveTween = transform.MoveLocalToZeroOfParent(position, 0.4f)
                .SetEase(Ease.OutQuart)
                .OnComplete(callback.Invoke);
        }

        public void BoosterActivated()
        {
            Debug.Log(nameof(BoosterActivated));
        }
    }
}