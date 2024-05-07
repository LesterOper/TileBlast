using CodeBase.CoreBase.GameStateModel;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using CodeBase.ApplicationBase.AudioManager;

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
            AudioManager.AudioManager.Inst.PlayMainCoreTheme();
        }

        private void OnPauseClick()
        {
            AudioManager.AudioManager.Inst.PlayButton();
            _gameModel.DialogsController.ShowPauseDialog();
        }
    }
}