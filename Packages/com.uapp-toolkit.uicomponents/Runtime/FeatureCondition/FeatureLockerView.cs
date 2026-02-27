using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace UAppToolkit.UIComponents.FeatureCondition
{
    public class FeatureLockerView : MonoBehaviour
    {
        [SerializeField] private FeatureConditionBase _condition;
        [SerializeField] private Image _lockerView;
        [SerializeField] private LocalizeStringEvent _descriptionLocalizationEvent;
        [SerializeField] private Selectable _selectable;
        [SerializeField] private List<GameObject> _disableWhenLocked;

        public bool IsEnable => _condition.IsEnable();
        public Image BackgroundImage => _lockerView;
        public event Action OnChanged;

        public void Init()
        {
            UpdateView();
            _condition.OnChanged -= ConditionChanged;
            _condition.OnChanged += ConditionChanged;
        }

        public void Init(FeatureConditionBase condition)
        {
            if (_condition)
                _condition.OnChanged -= ConditionChanged;

            _condition = condition;
            Init();
        }

        public void Deinit()
        {
            if (_condition)
            {
                _condition.OnChanged -= ConditionChanged;
            }
        }

        private void ConditionChanged(FeatureConditionBase obj)
        {
            UpdateView();
            OnChanged?.Invoke();
        }

        private void UpdateView()
        {
            _lockerView.gameObject.SetActive(!_condition.IsEnable());
            if (_selectable != null)
            {
                _selectable.interactable = _condition.IsEnable();
            }

            if (_descriptionLocalizationEvent != null)
            {
                _descriptionLocalizationEvent.StringReference = _condition.DescriptionLocalization;
            }

            foreach (var disableObject in _disableWhenLocked)
            {
                disableObject.SetActive(_condition.IsEnable());
            }
        }

        private void OnDestroy()
        {
            Deinit();
        }
    }
}