using System;
using UnityEngine;
using Zenject;

namespace CodeBase.CoreBase.AimTopBar
{
    public class AimTopBarView : MonoBehaviour
    {
        [SerializeField] private AimView aimPrefab;
        
        [Inject] private AimTopBarController _aimTopBarController;

        private void Start()
        {
            _aimTopBarController.InitializeLevelAim(aimPrefab, transform);
        }
    }
}