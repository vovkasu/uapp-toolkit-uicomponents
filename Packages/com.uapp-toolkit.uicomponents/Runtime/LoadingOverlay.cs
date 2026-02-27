using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;

namespace UAppToolkit.UIComponents
{
    public class LoadingOverlay : MonoBehaviour
    {
        public event Action OnShown;
        public event Action OnHidden;

        [SerializeField] private Animator _animator;
        [SerializeField] private GameObject _inputLocker;

        public bool IsVisible { get; private set; }
        public bool IsInputLocked
        {
            get => _inputLocker.activeSelf;
            set => _inputLocker.SetActive(value);
        }

        public IEnumerator Show()
        {
            IsVisible = false;
            IsInputLocked = true;

            if (_animator != null)
            {
                _animator.SetTrigger("Show");
            }

            yield return new WaitUntil(() => IsVisible);
        }

        public void Hide()
        {
            if (_animator != null)
            {
                _animator.SetTrigger("Hide");
            }
        }

        public void OnShowedAnimationEvent()
        {
            IsVisible = true;
            OnShown?.Invoke();
        }

        public void OnHiddenAnimationEvent()
        {
            IsVisible = false;
            IsInputLocked = false;
            OnHidden?.Invoke();
        }

#if UNITY_EDITOR
        [ShowNativeProperty] public bool IsVisibleDebug => IsVisible;
#endif
    }
}