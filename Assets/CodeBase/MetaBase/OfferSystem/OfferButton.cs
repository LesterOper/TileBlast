using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.MetaBase.OfferSystem
{
    public class OfferButton : MonoBehaviour
    {
        [SerializeField] private Button actionButton;
        [SerializeField] private TextMeshProUGUI timer;

        private OfferInGame _eventInGame;

        private void Start()
        {
            actionButton.onClick.AddListener(() =>
            {
                Debug.Log(_eventInGame.offerType);
            });
        }

        public void Initialize(OfferInGame eventInGame)
        {
            _eventInGame = eventInGame;
            timer.text = eventInGame.duration.ToString();
        }
    }
}