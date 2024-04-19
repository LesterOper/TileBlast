using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.CoreBase.Input
{
    public interface IInputSystem
    {
        public event Action<Vector2> OnClick;
        public void SetEnable(bool enable);

        public void SetEnableBySomeDialogsWhichOpen(bool enable);
    }
    
    public class CustomInputSystem : MonoBehaviour, IPointerClickHandler, IInputSystem, ITickable
    {
        [SerializeField] private float clickDelay;
        private float delay;
        private bool _isEnable;
        private bool _isNoOneDialogOpen = true;
        

        public void OnPointerClick(PointerEventData eventData)
        {
            if(!_isNoOneDialogOpen) return;
            if (_isEnable && delay > clickDelay)
            {
                delay = 0;
                OnClick?.Invoke(eventData.position);
            }
        }
        
        public event Action<Vector2> OnClick;
        
        public void SetEnable(bool enable)
        {
            _isEnable = enable;
        }

        public void SetEnableBySomeDialogsWhichOpen(bool enable)
        {
            _isNoOneDialogOpen = enable;
        }

        public void Tick()
        {
            delay += Time.deltaTime;
        }
    }
}