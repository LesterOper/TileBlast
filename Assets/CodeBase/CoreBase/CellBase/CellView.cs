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
        [SerializeField] private ParticleSystem effect;
        [SerializeField] private ParticleSystem smoke;

        [SerializeField] private ParticleSystem bombEffect;
        
        private Tween _moveTween;

        public Transform CellTransform => transform;

        public void Initialize(Sprite coloredCellSprite, Sprite itemSprite, Color colorForParticle)
        {
            _renderComponent.sprite = coloredCellSprite;
            _itemView.Initialize(itemSprite);
            if (effect != null)
            {
                var main = effect.main;
                main.startColor = new ParticleSystem.MinMaxGradient(colorForParticle);
            }

            if (smoke != null)
            {
                var main = smoke.main;
                main.startColor = new ParticleSystem.MinMaxGradient(new Color(colorForParticle.r, colorForParticle.g,
                    colorForParticle.b, 0.5f));
            }
        }

        public virtual void FadeCell(float endValue, float duration)
        {
            _renderComponent.DOFade(endValue, duration);
            _itemView.FadeOut(endValue, duration);
        }

        public void PlayEffect(Action callback)
        {
            FadeCell(0, 0.1f);
            effect.Play();
            DOVirtual.DelayedCall(effect.main.duration, callback.Invoke);
        }
        
        public void MoveToTargetCell(Vector2 position, Action callback)
        {
            _moveTween?.Kill();
            _moveTween = transform.MoveLocalToZeroOfParent(position, 0.13f)
                .SetEase(Ease.OutQuart)
                .OnComplete(callback.Invoke);
        }

        public SpriteRenderer R => _itemView.R;
        public void BoosterActivated()
        {
            Debug.Log(nameof(BoosterActivated));
        }

        public void PlayBombEffect(Action callback)
        {
            FadeCell(0, 0.1f);
            bombEffect.gameObject.SetActive(true);
            bombEffect.Play();
            DOVirtual.DelayedCall(effect.main.duration, () =>
            {
                bombEffect.gameObject.SetActive(false);
                callback.Invoke();
            });
        }
    }
}