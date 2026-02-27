using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Localization;

namespace UAppToolkit.UIComponents.FeatureCondition
{
    public abstract class FeatureConditionBase : ScriptableObject
    {
        protected bool IsInited;
        public event Action<FeatureConditionBase> OnChanged;

        public void Init()
        {
            InitInternal();
            Changed();
            IsInited = true;
        }

        protected abstract void InitInternal();
        public abstract bool IsEnable();
        public abstract LocalizedString DescriptionLocalization { get; }

        protected void Changed()
        {
            OnChanged?.Invoke(this);
        }

        public virtual void Deinit()
        {
            IsInited = false;
        }

        #region Debug

        [ShowNativeProperty]
        public bool IsInitedDebug => IsInited;


        [ShowNativeProperty]
        public string IsEnableDebug
        {
            get
            {
                if (!IsInited)
                    return "Not inited";
                return IsEnable() ? "Enable" : "Disable";
            }
        }
        #endregion
    }
}
