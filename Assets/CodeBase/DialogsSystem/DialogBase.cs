using System;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.DialogsSystem
{
    public abstract class DialogBase : MonoBehaviour
    {
        [SerializeField] protected Button closeButton;
        [SerializeField] protected Button closeArea;
        [SerializeField] protected Button confirm;
        [SerializeField] protected Button decline;

        public event Action OnDialogClosedEvent;

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
            gameObject.SetActive(true);
        }
        
        public virtual void CloseDialog()
        {
            ClearDialogData();
            gameObject.SetActive(false);
            OnDialogClosedEvent?.Invoke();
        }

        protected virtual void Confirm()
        {
            
        }

        protected virtual void Decline()
        {
            
        }

        protected virtual void ClearDialogData()
        {
            
        }
    }
}