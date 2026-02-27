using System;
using NaughtyAttributes;
using UnityEngine;

namespace UAppToolkit.UIComponents.RedPointNotification
{
    public abstract class RedPointNotificationModelBase : ScriptableObject
    {
        public event Action OnChangedState;
        public virtual bool IsVisible => IsVisibleInternal;
        protected abstract bool IsVisibleInternal { get; }

        public virtual void Init()
        {
            InitInternal();
            StateChanged();
        }

        protected abstract void InitInternal();

        protected void StateChanged()
        {
            OnChangedState?.Invoke();
        }

#if UNITY_EDITOR
        [ShowNativeProperty]
        private string IsVisibleEditor => Application.isPlaying ? IsVisible.ToString() : "none";
#endif
    }
}