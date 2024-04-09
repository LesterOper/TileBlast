using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace CodeBase.CoreBase.Counter
{
    public class LevelCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI levelField;

        private GlobalGameStateModel _gameStateModel;

        [Inject]
        private void Construct(GlobalGameStateModel gameStateModel)
        {
            _gameStateModel = gameStateModel;
        }

        private void Start()
        {
            levelField.text = $"{_gameStateModel.CurrentLevel}";
        }
    }
}