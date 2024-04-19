using System;
using DG.Tweening;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CodeBase.ApplicationBase
{
    public class AnimationSource
    {
        public static void Parabolla(Transform target, SpriteRenderer animatedObj, float time, Action callback)
        {
            SpriteRenderer animate = Object.Instantiate(animatedObj, animatedObj.transform.position, Quaternion.identity);
            animate.sortingOrder = 10;
            animate.maskInteraction = SpriteMaskInteraction.None;
            Sequence sequence = DOTween.Sequence();

            sequence.Append(animate.transform.DOScale(animate.transform.localScale * 2.0f, time * 0.2f));

            sequence.Append(animate.transform.DOScale(animate.transform.localScale * 1.2f, time * 0.5f));
            animate.transform.DOScale(Vector3.zero, time * 0.3f).SetDelay(time * 0.8f);

            Sequence sequenceMove = DOTween.Sequence();
            sequenceMove.Append(animate.transform.DOMoveY(target.position.y, time).SetEase(Ease.InQuart).OnComplete(() =>
            {
                callback?.Invoke();
                Object.Destroy(animate);
            }));
            sequenceMove.Join(animate.transform.DOMoveX(target.position.x + (target.position.x * 0.3f), time)
                .SetEase(Ease.Linear));
            sequenceMove.Append(animate.transform.DOMoveX(target.position.x, time).SetEase(Ease.Linear));
            
        }
        
        public static void ParabollaForExist(Transform target, SpriteRenderer animatedObj, float time, Action callback)
        {
            animatedObj.sortingOrder = 10;
            animatedObj.maskInteraction = SpriteMaskInteraction.None;
            Sequence sequence = DOTween.Sequence();

            sequence.Append(animatedObj.transform.DOScale(animatedObj.transform.localScale * 1.5f, time * 0.2f));

            sequence.Append(animatedObj.transform.DOScale(animatedObj.transform.localScale * 1.2f, time * 0.5f));
            animatedObj.transform.DOScale(Vector3.zero, time * 0.5f).SetDelay(time * 0.8f);

            Sequence sequenceMove = DOTween.Sequence();
            sequenceMove.Append(animatedObj.transform.DOMoveY(target.position.y, time).SetEase(Ease.InQuart).OnComplete(() =>
            {
                callback?.Invoke();
                Object.Destroy(animatedObj);
            }));
            sequenceMove.Join(animatedObj.transform.DOMoveX(target.position.x + (target.position.x * 0.3f), time)
                .SetEase(Ease.Linear));
            sequenceMove.Append(animatedObj.transform.DOMoveX(target.position.x, time).SetEase(Ease.Linear));
            
        }
    }
}