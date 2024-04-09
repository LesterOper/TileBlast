using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.CoreBase.AimTopBar
{
    public class AimView : MonoBehaviour
    {
        [SerializeField] private AimCounter aimCounter;

        [SerializeField] private Image aimIcon;
        [SerializeField] private Image completeIcon;

        private IAimSetter _levelAim;

        public void Initialize(IAimSetter levelAim, Sprite aimIconSprite)
        {
            _levelAim = levelAim;
            aimCounter.SetCounterSetter(levelAim);
            aimIcon.sprite = aimIconSprite;
        }

        public void UpdateAimCounter()
        {
            aimCounter.UpdateCounter();
            ChangeStateOfAim();
        }

        private void ChangeStateOfAim() => completeIcon.gameObject.SetActive(_levelAim.Value <= 0);
    }
}