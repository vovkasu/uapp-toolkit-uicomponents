using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UAppToolkit.UIComponents
{
    public class Dropdown : MonoBehaviour
    {
        public UnityEvent<Toggle> OnOptionSelected;
        public UnityEvent<bool> OnDropdownShowChanged;

        [SerializeField] private Toggle _showDropdownToggle;
        [SerializeField] private ToggleGroupPublicToggles _optionsToggleGroup;
        [SerializeField] private RectTransform _optionsContainer;
        [SerializeField] private Toggle _defaultOptionToggle;

        private List<Toggle> _optionsCache;
        private bool _isInitialized;
        private bool _isDropdownShownLastFrame;

        public bool IsDropdownShown
        {
            get => _showDropdownToggle.isOn;
            set => _showDropdownToggle.isOn = value;
        }

        public IReadOnlyList<Toggle> Options => _optionsCache ??= GetAllOptionsToggles();

        public Toggle SelectedOption => Options.FirstOrDefault(toggle => toggle.isOn);

        public void Init()
        {
            if (_isInitialized)
                return;

            _showDropdownToggle.onValueChanged.AddListener(OnShowDropdownToggleChanged);
            foreach (var toggle in Options)
            {
                toggle.SetIsOnWithoutNotify(toggle == _defaultOptionToggle);
                toggle.onValueChanged.AddListener(_ => OnOptionChanged(toggle));
            }

            SetDropdownShow(false, true);
            _isInitialized = true;
        }

        private void OnShowDropdownToggleChanged(bool isShown)
        {
            SetDropdownShow(isShown, false);
        }

        private void SetDropdownShow(bool isShown, bool silent)
        {
            _optionsContainer.gameObject.SetActive(isShown);
            if (!silent)
                OnDropdownShowChanged?.Invoke(isShown);
        }

        private List<Toggle> GetAllOptionsToggles()
            => _optionsToggleGroup.GetComponentsInChildren<Toggle>(true).ToList();

        private void OnOptionChanged(Toggle changedToggle)
        {
            if (changedToggle.isOn)
            {
                OnOptionSelected?.Invoke(changedToggle);
            }
        }
        
        protected void Update()
        {
            HandleAnyMouseDown();
        }

        private void HandleAnyMouseDown()
        {
            if (IsTouchBegan() || Input.GetMouseButtonUp(0))
                TryClose();

            _isDropdownShownLastFrame = IsDropdownShown;
            return;

            static bool IsTouchBegan()
            {
                return Input.touchSupported && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended;
            }
        }

        private void TryClose()
        {
            if (_isDropdownShownLastFrame && IsDropdownShown)
                IsDropdownShown = false;
        }
    }
}
