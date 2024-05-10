using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.InGameBoosters;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Purchasing;

namespace CodeBase.DialogsSystem.Shop
{
    [CreateAssetMenu(menuName = "Configs/" + nameof(ShopConfig), fileName = nameof(ShopConfig))]
    public class ShopConfig : ScriptableObject
    {
        public List<ShopItemData> shopItems;
        public List<OfferData> offers;

        public ShopItemData Get(ShopCoinType shopCoinType) =>
            shopItems.FirstOrDefault(local => local.coinType == shopCoinType);
        
        public ShopItemData Get(int ind) =>
            shopItems[ind];

        public OfferData Get() => offers[0];

    }

    [Serializable]
    public class ShopItemData
    {
        public PayoutType payoutType;
        public ShopCoinType coinType;
        public Sprite icon;
        public string cost;
        public int prizeAmount;
        public bool hasBonus;
        [ShowIf(nameof(hasBonus))]
        public int bonusAmount;
    }

    public enum ShopCoinType
    {
        [ShopCoinTypeAttribute("coin_1")]
        Coin1,
        [ShopCoinTypeAttribute("coin_2")]
        Coin2,
        [ShopCoinTypeAttribute("coin_3")]
        Coin3,
        [ShopCoinTypeAttribute("coin_4")]
        Coin4,
        [ShopCoinTypeAttribute("coin_5")]
        Coin5
    }
    
    public class ShopCoinTypeAttribute : Attribute
    {
        public string SKU;

        public ShopCoinTypeAttribute(string sku)
        {
            SKU = sku;
        }
    }

    [Serializable]
    public class OfferData
    {
        public PayoutType payoutType;
        public OfferType offerType;
        public List<OfferPrize> offerPrizes;

        public string cost; 
        [Serializable]
        public class OfferPrize
        {
            public OfferPrizeType offerPrizeType;
            [ShowIf(nameof(IsCoinPrize))]
            public Sprite coin;

            [HideIf(nameof(IsCoinPrize))]
            public InGameBoosterType inGameBoosterType;
            public int amount;

            public bool IsCoinPrize() => offerPrizeType == OfferPrizeType.Coin;
            
        }
    }
    
    public enum OfferType
    {
        FirstLaunchOffer
    }

    public enum OfferPrizeType
    {
        Coin, Booster
    }
}