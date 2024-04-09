using System;
using CodeBase.CoreBase;
using CodeBase.CoreBase.Configs;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.MetaBase
{
    public class MetaController : MonoBehaviour
    {
        [SerializeField] private Button play;

        private GlobalGameStateModel _globalGameStateModel;

        [Inject]
        private void Construct(GlobalGameStateModel gameStateModel)
        {
            _globalGameStateModel = gameStateModel;
        }
        private void Start()
        {
            play.onClick.AddListener(OnPlayClick);
        }

        private void OnPlayClick()
        {
            if (_globalGameStateModel.HealthSetter.HasHealth)
                SceneManager.LoadScene(2);
        }
    }
}