using System;
using CodeBase.ApplicationBase.AudioManager;
using CodeBase.CoreBase;
using CodeBase.CoreBase.Configs;
using CodeBase.DialogsSystem;
using CodeBase.MetaBase.Views;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.MetaBase
{
    public class MetaController : MonoBehaviour
    {
        [SerializeField] private PlayButton playButtonEasy;
        [SerializeField] private PlayButton playButtonNormal;
        [SerializeField] private PlayButton playButtonHard;
        
        [SerializeField] private Button settings;

        private PlayButton currentButton;
        
        private GlobalGameStateModel _globalGameStateModel;
        private MetaDialogController _metaDialogController;
        private LevelsConfig _levelsConfig;

        [Inject]
        private void Construct(GlobalGameStateModel gameStateModel, MetaDialogController metaDialogController, LevelsConfig levelsConfig)
        {
            _globalGameStateModel = gameStateModel;
            _metaDialogController = metaDialogController;
            _levelsConfig = levelsConfig;
        }
        private void Start()
        {
            AudioManager.Inst.PlayMainMetaTheme();
            var levelDifficultyType =
                _levelsConfig.GetLevel().levelData.levelDifficultyType;
            currentButton = levelDifficultyType == LevelDifficultyType.Easy ? playButtonEasy :
                levelDifficultyType == LevelDifficultyType.Normal ? playButtonNormal : playButtonHard;
            
            currentButton.Initialize(_globalGameStateModel.CurrentLevel);
            settings.onClick.AddListener(OnSettingsClick);
        }

        

        private void OnSettingsClick()
        {
            AudioManager.Inst.PlayButton();
            _metaDialogController.ShowSettingsDialog();
        }
    }
}