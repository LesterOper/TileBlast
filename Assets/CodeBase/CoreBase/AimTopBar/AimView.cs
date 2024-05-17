using CodeBase.CoreBase.Configs;
using CodeBase.CoreBase.Configs.LevelSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.CoreBase.AimTopBar
{
    public class AimView : MonoBehaviour
    {
        [SerializeField] private AimCounter aimCounter;

        [SerializeField] private Image aimIcon;
        [SerializeField] private Image completeIcon;
        [SerializeField] private TextMeshProUGUI clearAll;

        private IAimSetter _levelAim;
        private LevelType _levelType;

        public void Initialize(IAimSetter levelAim, Sprite aimIconSprite, LevelType levelType)
        {
            _levelAim = levelAim;
            _levelType = levelType;
            if (levelType != LevelType.CLEAR_FIELD)
            {
                aimCounter.SetCounterSetter(levelAim);
                aimIcon.sprite = aimIconSprite;
            }
            else
            {
                aimCounter.SetCounterSetter(levelAim);
                //aimCounter.gameObject.SetActive(false);
                aimIcon.gameObject.SetActive(false);
                clearAll.gameObject.SetActive(true);
            }
        }

        public void UpdateAimCounter()
        {
            aimCounter.UpdateCounter();
            ChangeStateOfAim();
        }

        private void ChangeStateOfAim()
        {
            if (_levelType != LevelType.CLEAR_FIELD)
                completeIcon.gameObject.SetActive(_levelAim.Value <= 0);
            else if(_levelAim.Value <= 0)
                clearAll.text = "Complete";
        }
    }
}