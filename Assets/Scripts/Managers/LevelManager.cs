using Commands;
using Controllers;
using Data.UnityObjects;
using Enums;
using Signals;
using UnityEngine;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        #region Self Variables

        #region Singleton

        public static LevelManager Instance { get; private set; }

        private void Singleton()
        {
            if (Instance != null) { Destroy(gameObject); return; }
            Instance = this;
        }

        #endregion
        
        #region Private Variables
        
        [SerializeField] private Transform levelHolder;
        private CD_Game _cdGame;
        private int _currentLevel;
        private OnLevelInitializeCommand _initializeCommand;
        private OnLevelClearCommand _clearCommand;
        
        #endregion
        
        #endregion
        
        private void Awake()
        {
            Singleton();
            _cdGame = Resources.Load<CD_Game>("Data/CD_Game");
            _currentLevel = SaveManager.Instance.LoadCurrentLevel();
            _initializeCommand = new OnLevelInitializeCommand(_cdGame);
            _clearCommand = new OnLevelClearCommand(levelHolder);
        }
        
        private void Start()
        {
            CoreUISignals.Instance.onOpenPanel.Invoke(UIPanelType.MainMenu);
        }
        
        private void OnEnable() => SubscribeEvents();
        
        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onLevelInitialize += OnLevelInitialize;
            CoreGameSignals.Instance.onNextLevel += OnNextLevel;
            CoreGameSignals.Instance.onRestartLevel += OnRestartLevel;
            CoreGameSignals.Instance.onReset += OnReset;
            CoreGameSignals.Instance.onGetCurrentLevel += OnGetCurrentLevel;
        }
        
        private void OnLevelInitialize()
        {
            CD_Level cdLevel = _initializeCommand.Execute(_currentLevel);
            GameObject levelViewPrefab = Resources.Load<GameObject>("Prefabs/LevelView/LevelView");
            GameObject levelViewObj = Instantiate(levelViewPrefab, levelHolder);
            LevelView levelView = levelViewObj.GetComponent<LevelView>();
            levelView.Initialize(cdLevel);
        }
        
        private void OnNextLevel()
        {
            _currentLevel++;
            SaveManager.Instance.SaveCurrentLevel(_currentLevel);
            CoreGameSignals.Instance.onReset.Invoke();
            CoreGameSignals.Instance.onLevelInitialize.Invoke();
        }
        
        private void OnRestartLevel()
        {
            CoreGameSignals.Instance.onReset.Invoke();
            CoreGameSignals.Instance.onLevelInitialize.Invoke();
        }
        
        private void OnReset() => _clearCommand.Execute();
        
        private int OnGetCurrentLevel() => _currentLevel;
        
        private void UnsubscribeEvents()
        {
            CoreGameSignals.Instance.onLevelInitialize -= OnLevelInitialize;
            CoreGameSignals.Instance.onNextLevel -= OnNextLevel;
            CoreGameSignals.Instance.onRestartLevel -= OnRestartLevel;
            CoreGameSignals.Instance.onReset -= OnReset;
            CoreGameSignals.Instance.onGetCurrentLevel -= OnGetCurrentLevel;
        }
        
        private void OnDisable() => UnsubscribeEvents();
    }
}