using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UAppToolkit.UIComponents
{
    public class ButtonLongPressCountChanger : MonoBehaviour
    {
        public ButtonLongPress Button;
        [Space]
        [SerializeField] private List<int> _values;

        public UnityEvent<int> OnLongPressWithCountAction;

        private int _currentActionIndex;
        private int _currentValue;

        public void Awake()
        {
            Button.OnLongPressStarted.AddListener(OnLongPressStarted);
            Button.OnLongPressAction.AddListener(OnLongPressAction);
        }

        private void OnLongPressStarted()
        {
            _currentActionIndex = -1;
        }

        private void OnLongPressAction()
        {
            _currentActionIndex++;
            _currentValue = GetCurrentValue();
            OnLongPressWithCountAction?.Invoke(_currentValue);
        }

        private int GetCurrentValue()
        {
            if (_values.Count == 0)
                return 1;

            var index = Mathf.Clamp(_currentActionIndex, 0, _values.Count - 1);
            return _values[index];
        }
    }
}
