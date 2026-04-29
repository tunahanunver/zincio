using System.Collections.Generic;
using Data.UnityObjects;
using Extensions;
using Signals;
using UnityEngine;

namespace Managers
{
    public class WordManager : MonoBehaviour
    {
        #region Self Variables

        #region Singleton

        public static WordManager Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance != null) { Destroy(gameObject); return; }
            Instance = this;
        }

        #endregion
        
        #region Private Variables
        
        private CD_WordList _wordList;
        private HashSet<string> _usedWords = new HashSet<string>();
        private string _currentRequiredLetter;
        private int _chainCount;
        
        #endregion
        
        #endregion
        
        private void OnEnable() => SubscribeEvents();
        
        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onLevelInitialize += OnLevelInitialize;
            WordSignals.Instance.onWordSubmitted += OnWordSubmitted;
            CoreGameSignals.Instance.onReset += OnReset;
            WordSignals.Instance.onGetRequiredLetter += OnGetRequiredLetter;
        }
        
        private void OnLevelInitialize()
        {
            int levelIndex = CoreGameSignals.Instance.onGetCurrentLevel();
            CD_Game cdGame = Resources.Load<CD_Game>("Data/CD_Game");
            CD_Level cdLevel = cdGame.levels[levelIndex % cdGame.levels.Count];
            _wordList = cdLevel.wordList;
            _usedWords.Clear();
            _chainCount = 0;
            string startWord = GetRandomStartWord();
            _usedWords.Add(startWord.NormalizeTurkish());
            _currentRequiredLetter = startWord.GetLastLetter().ToString().ToTurkishUpper();
            WordSignals.Instance.onWordAccepted.Invoke(startWord);
            WordSignals.Instance.onChainUpdated.Invoke(_chainCount);
        }
        
        private void OnWordSubmitted(string rawInput)
        {
            string normalized = rawInput.NormalizeTurkish();
            if (string.IsNullOrWhiteSpace(normalized))
            {
                WordSignals.Instance.onWordRejected.Invoke("Boş kelime girilemez.");
                return;
            }
            if (!normalized.StartsWith(_currentRequiredLetter.ToTurkishLower()))
            {
                WordSignals.Instance.onWordRejected.Invoke($"Kelime '{_currentRequiredLetter}' harfiyle başlamalı.");
                return;
            }
            if (_usedWords.Contains(normalized))
            {
                WordSignals.Instance.onWordRejected.Invoke("Bu kelime zaten kullanıldı.");
                return;
            }
            if (!IsValidWord(normalized))
            {
                WordSignals.Instance.onWordRejected.Invoke("Geçersiz kelime.");
                return;
            }
            _usedWords.Add(normalized);
            _chainCount++;
            _currentRequiredLetter = normalized.GetLastLetter().ToString().ToTurkishUpper();
            WordSignals.Instance.onWordAccepted.Invoke(rawInput.ToTurkishUpper());
            WordSignals.Instance.onChainUpdated.Invoke(_chainCount);
        }
        
        private bool IsValidWord(string normalized)
        {
            if (_wordList == null) return true;
            return _wordList.words.Contains(normalized);
        }
        
        private string GetRandomStartWord()
        {
            if (_wordList == null || _wordList.words.Count == 0)
                return "araba";
            int index = Random.Range(0, _wordList.words.Count);
            return _wordList.words[index].ToTurkishUpper();
        }
        
        private char OnGetRequiredLetter() =>
            string.IsNullOrEmpty(_currentRequiredLetter) ? '\0' : _currentRequiredLetter[0];
        
        private void OnReset()
        {
            _usedWords.Clear();
            _chainCount = 0;
            _currentRequiredLetter = string.Empty;
        }
        
        public int GetChainCount() => _chainCount;
        
        private void UnsubscribeEvents()
        {
            CoreGameSignals.Instance.onLevelInitialize -= OnLevelInitialize;
            WordSignals.Instance.onWordSubmitted -= OnWordSubmitted;
            CoreGameSignals.Instance.onReset -= OnReset;
            WordSignals.Instance.onGetRequiredLetter -= OnGetRequiredLetter;
        }
        
        private void OnDisable() => UnsubscribeEvents();
    }
}