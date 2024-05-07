using System;
using CodeBase.ApplicationBase.AudioManager;
using CodeBase.CoreBase.GameStateModel;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.DialogsSystem
{
    public abstract class DialogBase : MonoBehaviour
    {
        [SerializeField] protected Button closeButton;
        [SerializeField] protected Button closeArea;
        [SerializeField] protected Button confirm;
        [SerializeField] protected Button decline;
        
        public event Action OnDialogClosedEvent;
        public event Action OnDialogHideEvent;

        protected void Start()
        {
            if(closeButton!=null) 
                closeButton.onClick.AddListener(CloseDialog);
            if(closeArea != null) 
                closeArea.onClick.AddListener(CloseDialog);
            if(confirm != null)
                confirm.onClick.AddListener(Confirm);
            if(decline != null)
                decline.onClick.AddListener(Decline);
        }

        public virtual void Initialize()
        {
            Show();
        }

        public virtual void InitializePauseRestart(bool backHome)
        {
        }
        
        public virtual void CloseDialog()
        {
            AudioManager.Inst.PlayButton();
            Hide();
            OnDialogClosedEvent?.Invoke();
        }

        protected virtual void Confirm()
        {
            AudioManager.Inst.PlayButton();
        }

        protected virtual void Decline()
        {
            AudioManager.Inst.PlayButton();
            
        }

        protected virtual void Hide()
        {
            ClearDialogData();
            gameObject.SetActive(false);
        }

        protected virtual void ClearDialogData()
        {
            
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
        }
    }
}