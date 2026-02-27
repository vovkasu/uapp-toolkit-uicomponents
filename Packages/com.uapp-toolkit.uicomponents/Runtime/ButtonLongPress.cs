using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UAppToolkit.UIComponents
{
    public class ButtonLongPress : Button
    {
        [Header("Long press")]
        [Range(0.1f, 5f)]
        public float HoldDurationSec = 0.3f;
        public bool Repeat;
        [Range(0.1f, 3f)]
        public float RepeatPeriodSec = 0.1f;
        public UnityEvent OnLongPressAction;
        public UnityEvent OnLongPressStarted;
        public UnityEvent OnLongPressEnded;

        private bool _isPointerDown;
        private bool _isLongClickDone;
        private float _pressTime;
        private float _nextRepeatTime;

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            _isPointerDown = true;
            _isLongClickDone = false;
            _pressTime = 0f;
            _nextRepeatTime = 0f;
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            _isPointerDown = false;
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            _isPointerDown = false;
        }

        private void EndLongPressIfActive()
        {
            if (!_isLongClickDone)
                return;

            _isLongClickDone = false;
            OnLongPressEnded?.Invoke();
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (_isLongClickDone)
                return;

            base.OnPointerClick(eventData);
        }

        private void Update()
        {
            if (!_isPointerDown || !IsPressed())
            {
                EndLongPressIfActive();
                return;
            }

            _pressTime += Time.deltaTime;

            if (!_isLongClickDone && _pressTime >= HoldDurationSec)
            {
                _isLongClickDone = true;
                OnLongPressStarted?.Invoke();
                OnLongPressAction?.Invoke();
                _nextRepeatTime = _pressTime + RepeatPeriodSec;
            }
            else if (_isLongClickDone && Repeat && _pressTime >= _nextRepeatTime)
            {
                OnLongPressAction?.Invoke();
                _nextRepeatTime += RepeatPeriodSec;
            }
        }
    }
}
