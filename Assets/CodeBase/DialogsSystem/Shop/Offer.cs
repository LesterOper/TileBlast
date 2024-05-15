using System;
using System.Collections.Generic;
using CodeBase.InGameBoosters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.DialogsSystem.Shop
{
    public class Offer : MonoBehaviour
    {
        [SerializeField] private Button buy;
        [SerializeField] private TextMeshProUGUI cost;
        [SerializeField] private List<OfferPrizeItem> offerPrize;

        [Inject] private InGameBoostersConfig _inGameBoostersConfig;

        private OfferData _offerData;
        public event Action<OfferType> OnOfferBuyEvent;
        
        public void Initialize(OfferData offerData)
        {
            _offerData = offerData;
            cost.text = offerData.cost;

            var offersPrize = offerData.offerPrizes;

            for (int i = 0; i < offerPrize.Count; i++)
            {
                if (i >= offersPrize.Count)
                {
                    offerPrize[i].gameObject.SetActive(false);
                    continue;
                }
                OfferData.OfferPrize offerPrizeLocal = offersPrize[i];
                var icon = offerPrizeLocal.IsCoinPrize() ? 
                    offerPrizeLocal.coin : 
                    _inGameBoostersConfig.Get(offerPrizeLocal.inGameBoosterType);
                offerPrize[i].Initialize(icon, offerPrizeLocal.amount);
            }
            
            buy.onClick.AddListener(BuyClicked);
        }

        private void BuyClicked()
        {
            OnOfferBuyEvent?.Invoke(_offerData.offerType);   
        }
    }
}