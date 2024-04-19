using System;
using CodeBase.CoreBase.Configs;
using CodeBase.CoreBase.GameStateModel;
using TMPro;
using UnityEngine;
using Zenject;

namespace CodeBase.CoreBase.AimTopBar
{
    public class AimTopBarView : MonoBehaviour
    {
        [SerializeField] private AimView aimPrefab;

        [Inject] private AimTopBarController _aimTopBarController;
        [Inject] private GameModel _gameModel;

        private void Start()
        {
            _aimTopBarController.InitializeLevelAim(aimPrefab, transform);
        }
    }
}