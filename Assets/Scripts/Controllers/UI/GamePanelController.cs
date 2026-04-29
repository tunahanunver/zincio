using Data.UnityObjects;
using Signals;
using TMPro;
using UnityEngine;

namespace Controllers.UI
{
    public class GamePanelController : UIPanelController
    {
        #region Serialized Variables

        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private TextMeshProUGUI levelNameText;

        #endregion
        
        protected override void SubscribeEvents()
        {
            ScoreSignals.Instance.onScoreUpdated += OnScoreUpdated;
            TimerSignals.Instance.onTimerTick += OnTimerTick;
            UISignals.Instance.onSetLevelName += OnSetLevelName;
            CoreGameSignals.Instance.onLevelInitialize += OnLevelInitialize;
        }
        
        private void OnLevelInitialize()
        {
            int levelIndex = CoreGameSignals.Instance.onGetCurrentLevel();
            CD_Game cdGame = Resources.Load<CD_Game>("Data/CD_Game");
            CD_Level cdLevel = cdGame.levels[levelIndex % cdGame.levels.Count];
            levelNameText.text = (levelIndex + 1) + ": " + cdLevel.levelName;
            scoreText.text = "0";
            timerText.text = "1:00";
        }
        
        private void OnScoreUpdated(int score) => scoreText.text = score.ToString();
        
        private void OnTimerTick(float remaining)
        {
            int minutes = Mathf.FloorToInt(remaining / 60f);
            int seconds = Mathf.FloorToInt(remaining % 60f);
            timerText.text = string.Format("{0}:{1:00}", minutes, seconds);
        }
        
        private void OnSetLevelName(string name) => levelNameText.text = name;
        
        protected override void UnsubscribeEvents()
        {
            ScoreSignals.Instance.onScoreUpdated -= OnScoreUpdated;
            TimerSignals.Instance.onTimerTick -= OnTimerTick;
            UISignals.Instance.onSetLevelName -= OnSetLevelName;
            CoreGameSignals.Instance.onLevelInitialize -= OnLevelInitialize;
        }
    }
}