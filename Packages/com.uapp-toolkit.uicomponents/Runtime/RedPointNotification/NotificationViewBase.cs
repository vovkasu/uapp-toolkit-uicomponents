using System;
using UnityEngine;

namespace UAppToolkit.UIComponents.RedPointNotification
{
    public abstract class NotificationViewBase : MonoBehaviour
    {
        private bool _updateLate;
        public abstract bool IsVisible { get; }

        public event Action OnChangedState;

        public virtual void Init()
        {
            Deinit();
            InitInternal();
        }

        protected void InitInternal()
        {
            SubscribeChangeState();
            UpdateView();
        }

        protected void Deinit()
        {
            UnsubscribeChangeState();
        }
        
        protected void OnChangedModelState()
        {
            UpdateViewLate();
            OnChangedState?.Invoke();
        }

        protected abstract void SubscribeChangeState();
        protected abstract void UnsubscribeChangeState();

        protected abstract void UpdateView();

        protected void UpdateViewLate()
        {
            _updateLate = true;
        }

        private void LateUpdate()
        {
            if (_updateLate)
            {
                _updateLate = false;
                UpdateView();
            }
        }

        private void OnDestroy()
        {
            Deinit();
        }
    }
}