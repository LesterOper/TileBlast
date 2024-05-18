using System;
using System.Collections.Generic;
using CodeBase.DialogsSystem.Shop;
using UnityEngine;

namespace CodeBase.MetaBase.OfferSystem
{
    [CreateAssetMenu(menuName = "MetaConfigs/" + nameof(OffersInGameConfig), fileName = nameof(OffersInGameConfig))]
    public class OffersInGameConfig : ScriptableObject
    {
        public List<OfferInGame> offersInGame;

        public List<OfferInGame> GetActiveOffers() => offersInGame.FindAll(local => local.activeOffer);
    }

    [Serializable]
    public class OfferInGame
    {
        public OfferType offerType;
        public int duration;
        public OfferButton offerButtonPrefab;

        public bool activeOffer;
    }
    
}