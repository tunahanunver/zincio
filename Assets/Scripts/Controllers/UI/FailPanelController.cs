using System;
using Enums;
using Managers;
using Signals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers.UI
{
    public class FailPanelController : UIPanelController
    {
        #region Serialized Variables

        [SerializeField] private TextMeshProUGUI chainCountText;
        [SerializeField] private Image[] stars;

        #endregion

        private void Start()
        {
            int chainCount = WordManager.Instance.GetChainCount();
            chainCountText.text = "KELİME SAYISI: " + chainCount;
        }

        protected override void SubscribeEvents()
        {
            CoreGameSignals.Instance.onLevelFail += OnLevelFail;
        }
        
        private void OnLevelFail()
        {
            foreach (Image star in stars)
            {
                Color color = star.color;
                color.a = 0.3f;
                star.color = color;
            }
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