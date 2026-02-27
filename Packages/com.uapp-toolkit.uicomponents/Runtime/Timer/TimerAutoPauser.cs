using System;
using UnityEngine;

namespace UAppToolkit.UIComponents
{
    public class TimerAutoPauser : MonoBehaviour
    {
        [SerializeField] private TimerBase _timerBase;

        private TimeSpan _remainingBeforePause;
        private bool _isPaused;

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
                Pause();
            else
                Resume();
        }

        private void Pause()
        {
            if (!_timerBase.InProcess || _isPaused)
                return;

            _isPaused = true;
            _remainingBeforePause = _timerBase.LeftTime;
            _timerBase.StopTimer();
        }

        private void Resume()
        {
            if (!_isPaused)
                return;

            _isPaused = false;
            if (_remainingBeforePause > TimeSpan.Zero)
                _timerBase.StartTimer(_remainingBeforePause, _timerBase.UpdatePeriodSec);
        }
    }
}