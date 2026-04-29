using Data.UnityObjects;
using Signals;
using UnityEngine;

namespace Managers
{
    public class TimerManager : MonoBehaviour
    {
        #region Self Variables

        #region Singleton

        public static TimerManager Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance != null) { Destroy(gameObject); return; }
            Instance = this;
        }

        #endregion
        
        #region Private Variables
        
        private float _remainingTime;
        private bool _isRunning;
        private float _gameDuration;
        
        #endregion
        
        #endregion
        
        private void OnEnable() => SubscribeEvents();
        
        private void Update()
        {
            if (!_isRunning) return;
            _remainingTime -= Time.deltaTime;
            if (_remainingTime <= 0f)
            {
                _remainingTime = 0f;
                _isRunning = false;
                TimerSignals.Instance.onTimerTick.Invoke(_remainingTime);
                TimerSignals.Instance.onTimerEnd.Invoke();
                return;
            }
            TimerSignals.Instance.onTimerTick.Invoke(_remainingTime);
        }
        
        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onLevelInitialize += OnLevelInitialize;
            CoreGameSignals.Instance.onReset += OnReset;
        }
        
        private void OnLevelInitialize()
        {
            CD_Game cdGame = Resources.Load<CD_Game>("Data/CD_Game");
            _gameDuration = cdGame.gameDuration;
            _remainingTime = _gameDuration;
            _isRunning = false;
        }
        
        public void StartTimer() => _isRunning = true;
        
        public void StopTimer() => _isRunning = false;
        
        public void ResetTimer() => _remainingTime = _gameDuration;
        
        private void OnReset()
        {
            StopTimer();
            ResetTimer();
        }
        
        private void UnsubscribeEvents()
        {
            CoreGameSignals.Instance.onLevelInitialize -= OnLevelInitialize;
            CoreGameSignals.Instance.onReset -= OnReset;
        }
        
        private void OnDisable() => UnsubscribeEvents();
    }
}