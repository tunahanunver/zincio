using System;
using UnityEngine;
using UnityEngine.Events;

namespace Signals
{
    public class CoreGameSignals : MonoBehaviour
    {
        #region Singleton

        public static CoreGameSignals Instance;

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
        
        public UnityAction onLevelInitialize = delegate { };
        public UnityAction onLevelSuccess = delegate { };
        public UnityAction onLevelFail = delegate { };
        public UnityAction onNextLevel = delegate { };
        public UnityAction onRestartLevel = delegate { };
        public UnityAction onReset = delegate { };
        public Func<int> onGetCurrentLevel = delegate { return 0; };
    }
}