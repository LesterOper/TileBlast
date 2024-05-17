using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.DialogsSystem.Shop
{
    public class OfferPrizeItem : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI amountText;

        public void Initialize(Sprite iconPrize, int amount)
        {
            icon.sprite = iconPrize;
            icon.SetNativeSize();

            amountText.text = $"+{amount}";
            
        }
    }
}