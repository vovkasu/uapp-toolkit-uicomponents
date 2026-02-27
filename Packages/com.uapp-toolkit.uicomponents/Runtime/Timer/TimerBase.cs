using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace UAppToolkit.UIComponents
{
    public abstract class TimerBase : MonoBehaviour
    {
        public UnityEvent OnComplete;
        public UnityEvent<TimeSpan> OnUpdate;

        private DateTime _completedAt;
        private float _updatePeriodSec;
        private Coroutine _timerCoroutine;

        public bool InProcess => _timerCoroutine != null;
        public TimeSpan LeftTime => _completedAt - DateTime.Now;
        public float UpdatePeriodSec => _updatePeriodSec;

        protected abstract void UpdateText(TimeSpan leftTime);

        public void StartTimer(TimeSpan timeSpan, float updatePeriodSec = 1f)
        {
            Show();
            StopTimer();
            _completedAt = DateTime.Now + timeSpan;
            _updatePeriodSec = updatePeriodSec;
            _timerCoroutine = StartCoroutine(UpdateTimerAsync());
        }

        public void StopTimer()
        {
            if (_timerCoroutine == null)
            {
                return;
            }
            StopCoroutine(_timerCoroutine);
            _timerCoroutine = null;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        private IEnumerator UpdateTimerAsync()
        {
            var waitForSeconds = new WaitForSeconds(_updatePeriodSec);
            TimeSpan leftTime;
            do
            {
                leftTime = _completedAt - DateTime.Now;
                leftTime = leftTime < TimeSpan.Zero ? TimeSpan.Zero : leftTime;
                UpdateText(leftTime);
                OnUpdate?.Invoke(leftTime);
                yield return waitForSeconds;
            } while (leftTime.TotalSeconds > 0);

            Completed();
        }

        private void Completed()
        {
            OnComplete.Invoke();
        }
    }
}