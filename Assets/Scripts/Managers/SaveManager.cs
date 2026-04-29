using UnityEngine;

namespace Managers
{
    public class SaveManager : MonoBehaviour
    {
        #region Self Variables

        #region Singleton

        public static SaveManager Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance != null) { Destroy(gameObject); return; }
            Instance = this;
        }

        #endregion
        
        #region Private Variables
        
        private const string KeyCurrentLevel = "CurrentLevel";
        private const string KeyLevelStar = "LevelStar_";
        private const string KeyHighScore = "HighScore_";
        
        #endregion

        #endregion
        
        public void SaveCurrentLevel(int index)
        {
            PlayerPrefs.SetInt(KeyCurrentLevel, index);
            PlayerPrefs.Save();
        }
        public int LoadCurrentLevel() => PlayerPrefs.GetInt(KeyCurrentLevel, 0);
        
        public void SaveLevelStar(int levelIndex, int starCount)
        {
            int existing = LoadLevelStar(levelIndex);
            if (starCount > existing)
            {
                PlayerPrefs.SetInt(KeyLevelStar + levelIndex, starCount);
                PlayerPrefs.Save();
            }
        }
        
        public int LoadLevelStar(int levelIndex) =>
            PlayerPrefs.GetInt(KeyLevelStar + levelIndex, 0);
        
        public void SaveHighScore(int levelIndex, int score)
        {
            int existing = LoadHighScore(levelIndex);
            if (score > existing)
            {
                PlayerPrefs.SetInt(KeyHighScore + levelIndex, score);
                PlayerPrefs.Save();
            }
        }
        
        public int LoadHighScore(int levelIndex) =>
            PlayerPrefs.GetInt(KeyHighScore + levelIndex, 0);
        
        public bool IsLevelUnlocked(int levelIndex)
        {
            if (levelIndex == 0) return true;
            return LoadLevelStar(levelIndex - 1) >= 1;
        }
    }
}