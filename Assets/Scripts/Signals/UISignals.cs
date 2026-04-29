using UnityEngine;
using UnityEngine.Events;

namespace Signals
{
    public class UISignals : MonoBehaviour
    {
        #region Singleton

        public static UISignals Instance;

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
        
        public UnityAction<int> onSetLevelText = delegate { };
        public UnityAction<string> onSetLevelName = delegate { };
        public UnityAction<int> onSetScore = delegate { };
        public UnityAction<int> onSetHighScore = delegate { };
        public UnityAction<int> onSetChainCount = delegate { };
        public UnityAction<float> onSetTimerDisplay = delegate { };
    }
}