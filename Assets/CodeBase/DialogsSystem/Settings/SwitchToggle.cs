using System;
using System.Collections;
using CodeBase.ApplicationBase.AudioManager;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CodeBase.DialogsSystem.Settings
{
    public class SwitchToggle : MonoBehaviour, IPointerClickHandler
    {
        [Header("UI Components")]
        [SerializeField] private Image switcher;
        [SerializeField] private Image background;
        [SerializeField] private TextMeshProUGUI textOn;
        [SerializeField] private TextMeshProUGUI textOff;

        [Header("Sprites for switcher")] [SerializeField] private Sprite[] switcherSprites;
        [Header("Sprites for background")] [SerializeField] private Sprite[] backgroundSprites;

        private bool IsOn;

        public event Action<bool> OnSwitchClickedEvent; 

        public void OnPointerClick(PointerEventData eventData)
        {
            AudioManager.Inst.PlayButton();
            OnClick();
        }

        private void OnClick()
        {
            ChangeSwitcher();
        }

        public void Initialize(bool isOn)
        {
            IsOn = isOn;
            switcher.sprite = switcherSprites[IsOn ? 0 : 1];
            background.sprite = backgroundSprites[IsOn ? 0 : 1];
            switcher.rectTransform.DOAnchorPosX(IsOn ? -5 : -150, 0f);
            textOn.gameObject.SetActive(IsOn);
            textOff.gameObject.SetActive(!IsOn);
        }

        private void ChangeSwitcher()
        {
            IsOn = !IsOn;
            OnSwitchClickedEvent?.Invoke(IsOn);
            switcher.rectTransform.DOAnchorPosX(IsOn ? -5 : -150, 0.15f).SetEase(Ease.InOutQuart);
            StartCoroutine(SwapSwitcher(0.15f, IsOn ? 0 : 1));
            textOn.gameObject.SetActive(IsOn);
            textOff.gameObject.SetActive(!IsOn);
        }

        private IEnumerator SwapSwitcher(float duration, int index)
        {
            switcher.CrossFadeAlpha(0.3f, duration, false);
            background.CrossFadeAlpha(0.3f, duration, false);
            yield return new WaitForSeconds(duration/2);

            switcher.sprite = switcherSprites[index];
            switcher.CrossFadeAlpha(1, duration, false);
            background.sprite = backgroundSprites[index];
            background.CrossFadeAlpha(1, duration, false);
            
        }
    }
}