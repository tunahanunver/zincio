using System;
using Data.ValueObjects;
using UnityEngine;
using UnityEngine.Events;

namespace Signals
{
    public class WordSignals : MonoBehaviour
    {
        #region Singleton

        public static WordSignals Instance;

        private void Awake()
        {
            if (Instance != null  && Instance != this)
            { 
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        #endregion
        
        public UnityAction<string> onWordSubmitted = delegate { };
        public UnityAction<string> onWordAccepted = delegate { };
        public UnityAction<string> onWordRejected = delegate { };
        public UnityAction<int> onChainUpdated = delegate { };
        public UnityAction<string> onStartWordSet = delegate { };
        public Func<char> onGetRequiredLetter = delegate { return '\0'; };
        public Func<LetterSetData> onGetCurrentLetterSet = delegate { return default; };
        public UnityAction onLetterSetConsumed = delegate { };
    }
}