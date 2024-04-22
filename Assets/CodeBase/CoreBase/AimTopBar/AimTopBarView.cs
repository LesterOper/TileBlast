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
        [SerializeField] private Transform layout;

        [Inject] private AimTopBarController _aimTopBarController;

        private void Start()
        {
            _aimTopBarController.InitializeLevelAim(aimPrefab, layout);
        }
    }
}