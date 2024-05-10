using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.DialogsSystem.Shop
{
    public class ShopItem : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private Button buy;
        [SerializeField] private TextMeshProUGUI mainPrize;
        [SerializeField] private TextMeshProUGUI cost;
        [SerializeField] private TextMeshProUGUI bonus;

        private ShopItemData _shopItemData;

        public event Action<ShopCoinType> OnBuyClickedEvent;

        public void Initialize(ShopItemData shopItemData)
        {
            _shopItemData = shopItemData;
            icon.sprite = _shopItemData.icon;
            icon.SetNativeSize();

            cost.text = $"{_shopItemData.cost}";

            mainPrize.text = _shopItemData.prizeAmount.ToString();
            if (_shopItemData.hasBonus)
                bonus.text = $"+{_shopItemData.bonusAmount} more";
            else bonus.gameObject.SetActive(false);
            
            buy.onClick.AddListener(()=>OnBuyClickedEvent?.Invoke(_shopItemData.coinType));
        }
        
    }
}