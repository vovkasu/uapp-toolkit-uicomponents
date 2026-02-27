using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UAppToolkit.UIComponents
{
    public class ProgressBarWithTween : MonoBehaviour
    {
        public event Action TweenProgressCompleted;

        [SerializeField] private Slider _slider;
        [SerializeField] private TextMeshProUGUI _txtProgress;
        [SerializeField] private string _progressTextFormat = "{0}/{1}";

        [Header("Animations")]
        [SerializeField] private float _progressChangeTime = 0.5f;
        [SerializeField] private Animator _progressBarAnimator;
        [SerializeField] private string _changingProgressBarTriggerName;
        [SerializeField] private string _resetTriggerName;

        private Coroutine _progressChangeCoroutine;
        private float _curValue;
        private float _targetValue;
        private float _maxValue = 1.0f;

        public float MaxValue => _maxValue;
        public float CurrentValue => _curValue;
        public float TargetValue => _targetValue;
        public bool IsTweening => _progressChangeCoroutine != null;

        public void SetProgress(float progress, bool withAnimation = true)
        {
            SetProgress(progress, _maxValue, withAnimation);
        }

        public void SetProgress(float value, float maxValue, bool withAnimation = true)
        {
            _maxValue = maxValue;
            _targetValue = value;

            if (_txtProgress)
                _txtProgress.text = string.Format(_progressTextFormat, Mathf.FloorToInt(_targetValue), Mathf.FloorToInt(_maxValue));

            float clampedTargetValue = Mathf.Clamp01(_targetValue / _maxValue);
            ChangeProgressBarValue(_curValue, clampedTargetValue, withAnimation);
        }

        private void ChangeProgressBarValue(float fromValue, float toValue, bool withAnimation)
        {
            StopProgressChangeCoroutine();

            if (!withAnimation || Mathf.Approximately(fromValue, toValue))
            {
                _curValue = toValue;
                _slider.value = toValue;
                return;
            }

            _progressChangeCoroutine = StartCoroutine(ChangeProgressBarValueCoroutine(fromValue, toValue));
        }

        private IEnumerator ChangeProgressBarValueCoroutine(float fromValue, float toValue)
        {
            if (_progressBarAnimator != null && !string.IsNullOrEmpty(_changingProgressBarTriggerName))
                _progressBarAnimator.SetTrigger(_changingProgressBarTriggerName);

            float time = 0.0f;
            while (time < _progressChangeTime)
            {
                time += Time.deltaTime;
                _curValue = Mathf.Lerp(fromValue, toValue, time / _progressChangeTime);
                _slider.value = _curValue;

                yield return null;
            }

            _curValue = toValue;
            _slider.value = toValue;
            _progressChangeCoroutine = null;
            TweenProgressCompleted?.Invoke();
        }

        private void StopProgressChangeCoroutine()
        {
            if (_progressChangeCoroutine != null)
            {
                StopCoroutine(_progressChangeCoroutine);
                _progressChangeCoroutine = null;
            }
        }
    }
}
