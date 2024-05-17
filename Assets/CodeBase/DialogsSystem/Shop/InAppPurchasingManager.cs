using System;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using Zenject;

namespace CodeBase.DialogsSystem.Shop
{
    public class InAppPurchasingManager : IDetailedStoreListener, IInitializable
    {
        private IStoreController _controller;
        private IExtensionProvider _extensionProvider;
        [Inject] private ShopConfig _shopConfig;

        public event Action<Product> buyComplete;
        public event Action<Product> purchaseFailed;

        public void OnInitializeFailed(InitializationFailureReason error)
        {
          
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            buyComplete?.Invoke(purchaseEvent.purchasedProduct);
            
            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            purchaseFailed?.Invoke(product);
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            _controller = controller;
            _extensionProvider = extensions;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
        }

        public void Init()
        {
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            for (int i = 0; i < _shopConfig.shopItems.Count; i++)
            {
                ShopItemData item = _shopConfig.shopItems[i];
                PayoutDefinition payout =
                    new PayoutDefinition(item.payoutType, item.coinType.ToString(), item.prizeAmount + item.bonusAmount);
                builder.AddProduct(item.coinType.GetAttribute<ShopCoinTypeAttribute>().SKU, ProductType.Consumable,
                    new IDs(), payout);
            }
            
            for (int i = 0; i < _shopConfig.offers.Count; i++)
            {
                OfferData item = _shopConfig.offers[i];
                PayoutDefinition payout =
                    new PayoutDefinition(item.payoutType, item.offerType.ToString(), 1);
                builder.AddProduct(item.offerType.GetAttribute<ShopCoinTypeAttribute>().SKU, ProductType.NonConsumable,
                    new IDs(), payout);
            }
            
            StandardPurchasingModule.Instance().useFakeStoreAlways = true;
            StandardPurchasingModule.Instance().useFakeStoreUIMode = FakeStoreUIMode.StandardUser;
            UnityPurchasing.Initialize(this, builder);
        }
        
        public void MakePurchase(string prodId)
        {
            _controller.InitiatePurchase(prodId);
        }
        

        public void Initialize()
        {
            Init();
        }
    }
}