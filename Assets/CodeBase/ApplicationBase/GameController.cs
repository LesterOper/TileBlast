using System;
using CodeBase.CoreBase.GameStateModel;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.ApplicationBase
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private Button pause;

        private GameModel _gameModel;

        [Inject]
        private void Construct(GameModel gameModel)
        {
            _gameModel = gameModel;
        }

        private void Start()
        {
            pause.onClick.AddListener(OnPauseClick);
        }

        private void OnPauseClick()
        {
            _gameModel.DialogsController.ShowPauseDialog();
        }
    }
}