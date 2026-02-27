using System;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.UI;

namespace UAppToolkit.UIComponents.PageSelector
{
    public class PageSelector : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private LocalizeStringEvent _title;
        [SerializeField] private GameObject _activeState;
        [SerializeField] private GameObject _inactiveState;
        [SerializeField] private Button _button;
        [Space]
        [SerializeField] private GameObject _notificationView;
        [SerializeField] private GameObject _additionalNotificationView;
        [Space]
        [SerializeField] private string _titleArgument = "number";
 
        public RectTransform RectTransform => _rectTransform;
        public bool HasNotification => HasMainNotification || HasAdditionalNotification;
        public int Index { get; private set; }
        public bool HasMainNotification { get; private set; }
        public bool HasAdditionalNotification { get; private set; }

        private Action<int> _onClick;

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClick);
        }

        public void Init(int index, bool isActive, Action<int> onClick)
        {
            Index = index;
            _onClick = onClick;

            var titleCopy = new LocalizedString(_title.StringReference.TableReference, _title.StringReference.TableEntryReference);
            titleCopy.Add(_titleArgument, new IntVariable { Value =  index + 1 });
            _title.StringReference = titleCopy;

            SetActive(isActive);
        }

        public void SetWidth(float width)
        {
            _rectTransform.sizeDelta = new Vector2(width, _rectTransform.sizeDelta.y);
        }

        public void SetActive(bool isActive)
        {
            _activeState.SetActive(isActive);
            _inactiveState.SetActive(!isActive);
        }

        public void SetRedPointNotification(bool isActive)
        {
            if (_notificationView == null)
                return;

            if (HasMainNotification == isActive)
                return;

            HasMainNotification = isActive;
            _notificationView.SetActive(isActive);
        }

        public void SetAdditionalRedPointNotification(bool isActive)
        {
            if (_additionalNotificationView == null)
                return;

            if (HasAdditionalNotification == isActive)
                return;

            HasAdditionalNotification = isActive;
            _additionalNotificationView.SetActive(isActive);
        }

        private void OnClick()
        {
            _onClick?.Invoke(Index);
        }
    }
}