using Data.UnityObjects;
using Data.ValueObjects;
using Enums;
using Managers;
using Signals;
using TMPro;
using UnityEngine;

namespace Controllers.UI
{
    public class WinPanelController : UIPanelController
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI highScoreText;
        [SerializeField] private TextMeshProUGUI chainCountText;
        [SerializeField] private GameObject[] stars;

        #endregion

        #endregion
        
        protected override void SubscribeEvents()
        {
            CoreGameSignals.Instance.onLevelSuccess += OnLevelSuccess;
        }
        
        private void OnLevelSuccess()
        {
            int levelIndex = CoreGameSignals.Instance.onGetCurrentLevel();
            int score = ScoreManager.Instance.GetCurrentScore();
            int highScore = SaveManager.Instance.LoadHighScore(levelIndex);
            int chainCount = WordManager.Instance.GetChainCount();
            scoreText.text = "SCORE: " + score;
            highScoreText.text = "HIGH SCORE: " + highScore;
            chainCountText.text = "CHAIN: " + chainCount + " WORDS";
            CD_Game cdGame = Resources.Load<CD_Game>("Data/CD_Game");
            CD_Level cdLevel = cdGame.levels[levelIndex % cdGame.levels.Count];
            int stars = CalculateStars(chainCount, cdLevel.starThresholds);
            ShowStars(stars);
        }
        
        private int CalculateStars(int chainCount, StarThresholdData thresholds)
        {
            if (chainCount >= thresholds.threeStar) return 3;
            if (chainCount >= thresholds.twoStar) return 2;
            if (chainCount >= thresholds.oneStar) return 1;
            return 0;
        }
        
        private void ShowStars(int count)
        {
            for (int i = 0; i < stars.Length; i++)
                stars[i].SetActive(i < count);
        }
        
        public void OnNextLevelClicked()
        {
            CoreUISignals.Instance.onCloseAllPanels.Invoke();
            CoreGameSignals.Instance.onNextLevel.Invoke();
        }
        
        public void OnMenuClicked()
        {
            CoreUISignals.Instance.onCloseAllPanels.Invoke();
            CoreUISignals.Instance.onOpenPanel.Invoke(UIPanelType.MainMenu);
        }
        
        protected override void UnsubscribeEvents()
        {
            CoreGameSignals.Instance.onLevelSuccess -= OnLevelSuccess;
        }
    }
}