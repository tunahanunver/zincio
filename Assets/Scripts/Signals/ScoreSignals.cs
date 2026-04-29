using UnityEngine;
using UnityEngine.Events;

namespace Signals
{
    public class ScoreSignals : MonoBehaviour
    {
        #region Singleton

        public static ScoreSignals Instance;

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
        
        public UnityAction<int> onScoreUpdated = delegate { };
        public UnityAction onHighScoreBeaten = delegate { };
    }
}