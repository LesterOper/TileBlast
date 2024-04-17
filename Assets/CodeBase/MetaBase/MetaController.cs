using System;
using CodeBase.CoreBase;
using CodeBase.CoreBase.Configs;
using CodeBase.DialogsSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.MetaBase
{
    public class MetaController : MonoBehaviour
    {
        [SerializeField] private Button play;
        [SerializeField] private Button settings;

        private GlobalGameStateModel _globalGameStateModel;
        private MetaDialogController _metaDialogController;

        [Inject]
        private void Construct(GlobalGameStateModel gameStateModel, MetaDialogController metaDialogController)
        {
            _globalGameStateModel = gameStateModel;
            _metaDialogController = metaDialogController;
        }
        private void Start()
        {
            play.onClick.AddListener(OnPlayClick);
            settings.onClick.AddListener(OnSettingsClick);
        }

        private void OnPlayClick()
        {
            if (_globalGameStateModel.HealthSetter.HasHealth)
            {
                SceneManager.LoadScene(2);
            }
            else _metaDialogController.ShowBuyMoreLivesDialog();
        }

        private void OnSettingsClick()
        {
            _metaDialogController.ShowSettingsDialog();
        }
    }
}