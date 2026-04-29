using Data.UnityObjects;
using Signals;
using TMPro;
using UnityEngine;

namespace Controllers
{
    public class LevelView : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private TextMeshProUGUI levelTitleText;

        #endregion
        
        #region Private Variables
        
        private CD_Level _cdLevel;
        
        #endregion

        #endregion
        
        public void Initialize(CD_Level cdLevel)
        {
            _cdLevel = cdLevel;
            if (levelTitleText != null)
                levelTitleText.text = cdLevel.levelName;
            UISignals.Instance.onSetLevelName.Invoke(
                (cdLevel.levelIndex + 1) + ": " + cdLevel.levelName
            );
        }
    }
}