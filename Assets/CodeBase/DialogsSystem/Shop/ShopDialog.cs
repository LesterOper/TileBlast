using System.Collections.Generic;
using CodeBase.CoreBase;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.DialogsSystem.Shop
{
    public class ShopDialog : DialogBase
    {
        [SerializeField] private ShopConfig _shopConfig;
        [SerializeField] private GameObject settingsButton;
        [SerializeField] private Offer offer;

        [SerializeField] private List<ShopItem> _shopItems;
        private GlobalGameStateModel _globalGameStateModel;
        private InAppPurchasingManager _inAppPurchasingManager;

        [Inject]
        private void Construct(GlobalGameStateModel globalGameStateModel,
            InAppPurchasingManager inAppPurchasingManager)
        {
            _globalGameStateModel = globalGameStateModel;
            _inAppPurchasingManager = inAppPurchasingManager;
        }

        public override void Initialize()
        {
            settingsButton.SetActive(false);
            closeButton.gameObject.SetActive(true);

            _inAppPurchasingManager.buyComplete += BuyComplete;
            for (int i = 0; i < _shopItems.Count; i++)
            {
                _shopItems[i].Initialize(_shopConfig.Get(i));
                _shopItems[i].OnBuyClickedEvent += OnBuy;
            }
            
            offer.Initialize(_shopConfig.Get());
            base.Initialize();
        }

        private void OnBuy(ShopCoinType shopCoinType)
        {
            Debug.Log("Buy - " + shopCoinType);
            _inAppPurchasingManager.MakePurchase(shopCoinType.GetAttribute<ShopCoinTypeAttribute>().SKU);
        }

        private void BuyComplete(int value)
        {
            _globalGameStateModel.CurrencySetter.Value = value;

        }

        public override void CloseDialog()
        {
            settingsButton.SetActive(true);
            closeButton.gameObject.SetActive(false);
            base.CloseDialog();
        }
    }
}