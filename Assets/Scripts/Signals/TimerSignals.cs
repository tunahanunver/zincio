using UnityEngine;
using UnityEngine.Events;

namespace Signals
{
    public class TimerSignals : MonoBehaviour
    {
        #region Singleton

        public static TimerSignals Instance;

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
        
        public UnityAction<float> onTimerTick = delegate { };
        public UnityAction onTimerEnd = delegate { };
    }
}