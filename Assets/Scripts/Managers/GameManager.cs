using Data.UnityObjects;
using Data.ValueObjects;
using Enums;
using Signals;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    { 
        #region Self Variables

        #region Singleton

        public static GameManager Instance { get; private set; }
        private void Awake()
        {
            if (Instance != null) { Destroy(gameObject); return; }
            Instance = this;
        }

        #endregion
        
        #region Private Variables
        
        private GameState _gameState = GameState.Idle;
        
        #endregion
        
        #endregion
        
        private void OnEnable() => SubscribeEvents();
        
        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onLevelInitialize += OnLevelInitialize;
            CoreGameSignals.Instance.onLevelSuccess += OnLevelSuccess;
            CoreGameSignals.Instance.onLevelFail += OnLevelFail;
            CoreGameSignals.Instance.onReset += OnReset;
            TimerSignals.Instance.onTimerEnd += OnTimerEnd;
            WordSignals.Instance.onWordAccepted += OnWordAccepted;
        }
        
        private void OnLevelInitialize() => SetState(GameState.Idle);
        
        private void OnWordAccepted(string word)
        {
            if (_gameState == GameState.Idle)
            {
                SetState(GameState.Playing);
                TimerManager.Instance.StopTimer();
            }
        }
        
        private void OnTimerEnd()
        {
            if (_gameState != GameState.Playing) return;
            int chainCount = WordManager.Instance.GetChainCount();
            CD_Game cdGame = Resources.Load<CD_Game>("Data/CD_Game");
            int levelIndex = CoreGameSignals.Instance.onGetCurrentLevel();
            CD_Level cdLevel = cdGame.levels[levelIndex % cdGame.levels.Count];
            StarThresholdData thresholds = cdLevel.starThresholds;
            if (chainCount >= thresholds.oneStar)
                CoreGameSignals.Instance.onLevelSuccess.Invoke();
            else
                CoreGameSignals.Instance.onLevelFail.Invoke();
        }
        
        private void OnLevelSuccess()
        {
            SetState(GameState.Win);
            TimerManager.Instance.StopTimer();
            int chainCount = WordManager.Instance.GetChainCount();
            int levelIndex = CoreGameSignals.Instance.onGetCurrentLevel();
            CD_Game cdGame = Resources.Load<CD_Game>("Data/CD_Game");
            CD_Level cdLevel = cdGame.levels[levelIndex % cdGame.levels.Count];
            StarThresholdData thresholds = cdLevel.starThresholds;
            int stars = CalculateStars(chainCount, thresholds);
            SaveManager.Instance.SaveLevelStar(levelIndex, stars);
            CoreUISignals.Instance.onOpenPanel.Invoke(UIPanelType.Win);
        }
        
        private void OnLevelFail()
        {
            SetState(GameState.Fail);
            TimerManager.Instance.StopTimer();
            CoreUISignals.Instance.onOpenPanel.Invoke(UIPanelType.Fail);
        }
        
        private void OnReset() => SetState(GameState.Idle);
        
        private void SetState(GameState newState) => _gameState = newState;
        
        public GameState GetState() => _gameState;
        
        private int CalculateStars(int chainCount, StarThresholdData thresholds)
        {
            if (chainCount >= thresholds.threeStar) return 3;
            if (chainCount >= thresholds.twoStar) return 2;
            if (chainCount >= thresholds.oneStar) return 1;
            return 0;
        }
        
        private void UnsubscribeEvents()
        {
            CoreGameSignals.Instance.onLevelInitialize -= OnLevelInitialize;
            CoreGameSignals.Instance.onLevelSuccess -= OnLevelSuccess;
            CoreGameSignals.Instance.onLevelFail -= OnLevelFail;
            CoreGameSignals.Instance.onReset -= OnReset;
            TimerSignals.Instance.onTimerEnd -= OnTimerEnd;
            WordSignals.Instance.onWordAccepted -= OnWordAccepted;
        }
        
        private void OnDisable() => UnsubscribeEvents();
    }
}