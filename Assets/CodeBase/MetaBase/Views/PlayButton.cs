using CodeBase.CoreBase;
using CodeBase.CoreBase.Configs;
using CodeBase.DialogsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.MetaBase.Views
{
    public class PlayButton : MonoBehaviour
    {
        [SerializeField] private Button btnComponent;
        [SerializeField] private TextMeshProUGUI levelText;

        private GlobalGameStateModel _globalGameStateModel;
        private MetaDialogController _metaDialogController;

        [Inject]
        private void Construct(GlobalGameStateModel gameStateModel, MetaDialogController metaDialogController)
        {
            _globalGameStateModel = gameStateModel;
            _metaDialogController = metaDialogController;
        }
        
        public void Initialize(int levelNumber)
        {
            btnComponent.onClick.AddListener(OnPlayClick);
            levelText.text = $"Level {levelNumber}";
            gameObject.SetActive(true);
        }
        
        private void OnPlayClick()
        {
            if (_globalGameStateModel.HealthSetter.HasHealth)
            {
                SceneManager.LoadScene(2);
            }
            else _metaDialogController.ShowBuyMoreLivesDialog();
        }
    }
}