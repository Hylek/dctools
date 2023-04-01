using System;
using UnityEngine;

namespace DCTools
{
    public class GenericTimer
    {
        public event Action TimerComplete, TimerPaused, TimerStopped;

        private float _startTime, _currentTime;
        private bool _isActive;

        public GenericTimer(float startTime)
        {
            _currentTime = startTime;
            _isActive = false;
        }

        public void UpdateStartTime(float time) => _startTime = time;

        public void Update()
        {
            if (!_isActive) return;

            if (_currentTime >= 0)
            {
                _currentTime -= Time.deltaTime;
                if (_currentTime <= 0)
                {
                    _isActive = false;
                    TimerComplete?.Invoke();
                }
            }
        }

        public float GetCurrentTime() => _currentTime;

        public bool IsActive() => _isActive;

        public void Start()
        {
            if (_currentTime <= 0)
            {
                _currentTime = _startTime;
            }
        }

        public void Pause()
        {
            _isActive = false;
            TimerPaused?.Invoke();
        }

        public void Stop()
        {
            _isActive = false;
            _currentTime = _startTime;
            TimerStopped?.Invoke();
        }
    }
}
