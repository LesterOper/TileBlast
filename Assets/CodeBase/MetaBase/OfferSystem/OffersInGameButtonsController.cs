using UnityEngine;
using Zenject;

namespace CodeBase.MetaBase.OfferSystem
{
    public class OffersInGameButtonsController : MonoBehaviour
    {
        private OffersInGameConfig _offersInGameConfig;

        [Inject]
        private void Construct(OffersInGameConfig offersInGameConfig)
        {
            _offersInGameConfig = offersInGameConfig;
        }

        private void Start()
        {
            var offers = _offersInGameConfig.GetActiveOffers();

            for (int i = 0; i < offers.Count; i++)
            {
                OfferButton eventButton = Instantiate(offers[i].offerButtonPrefab, transform);
                eventButton.Initialize(offers[i]);
            }
        }
    }
}