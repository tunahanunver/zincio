using Signals;
using UnityEngine;

namespace Managers
{
    public class ScoreManager : MonoBehaviour
    {
        #region Self Variables

        #region Singleton

        public static ScoreManager Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance != null) { Destroy(gameObject); return; }
            Instance = this;
        }

        #endregion

        #region Private Variables

        private int _currentScore;
        private int _currentLevelIndex;
        [SerializeField] private const int ScoreMultiplier = 10;

        #endregion
        
        #endregion
        
        private void OnEnable() => SubscribeEvents();
        
        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onLevelInitialize += OnLevelInitialize;
            WordSignals.Instance.onWordAccepted += OnWordAccepted;
            CoreGameSignals.Instance.onLevelSuccess += OnLevelSuccess;
            CoreGameSignals.Instance.onReset += OnReset;
        }
        
        private void OnLevelInitialize()
        {
            _currentLevelIndex = CoreGameSignals.Instance.onGetCurrentLevel();
            _currentScore = 0;
            ScoreSignals.Instance.onScoreUpdated.Invoke(_currentScore);
        }
        
        private void OnWordAccepted(string word)
        {
            _currentScore += word.Length * ScoreMultiplier;
            ScoreSignals.Instance.onScoreUpdated.Invoke(_currentScore);
            int highScore = SaveManager.Instance.LoadHighScore(_currentLevelIndex);
            if (_currentScore > highScore)
                ScoreSignals.Instance.onHighScoreBeaten.Invoke();
        }
        
        private void OnLevelSuccess()
        {
            SaveManager.Instance.SaveHighScore(_currentLevelIndex, _currentScore);
        }
        
        private void OnReset() => _currentScore = 0;
        
        public int GetCurrentScore() => _currentScore;
        
        private void UnsubscribeEvents()
        {
            CoreGameSignals.Instance.onLevelInitialize -= OnLevelInitialize;
            WordSignals.Instance.onWordAccepted -= OnWordAccepted;
            CoreGameSignals.Instance.onLevelSuccess -= OnLevelSuccess;
            CoreGameSignals.Instance.onReset -= OnReset;
        }
        
        private void OnDisable() => UnsubscribeEvents();
    }
}