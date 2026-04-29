using Enums;
using Managers;
using Signals;
using TMPro;
using UnityEngine;

namespace Controllers.UI
{
    public class FailPanelController : UIPanelController
    {
        #region Serialized Variables

        [SerializeField] private TextMeshProUGUI chainCountText;

        #endregion
        
        protected override void SubscribeEvents()
        {
            CoreGameSignals.Instance.onLevelFail += OnLevelFail;
        }
        
        private void OnLevelFail()
        {
            int chainCount = WordManager.Instance.GetChainCount();
            chainCountText.text = "KELİME SAYISI: " + chainCount;
        }
        
        public void OnRetryClicked()
        {
            CoreUISignals.Instance.onCloseAllPanels.Invoke();
            CoreGameSignals.Instance.onRestartLevel.Invoke();
        }
        
        public void OnMenuClicked()
        {
            CoreUISignals.Instance.onCloseAllPanels.Invoke();
            CoreUISignals.Instance.onOpenPanel.Invoke(UIPanelType.MainMenu);
        }
        
        protected override void UnsubscribeEvents()
        {
            CoreGameSignals.Instance.onLevelFail -= OnLevelFail;
        }
    }
}