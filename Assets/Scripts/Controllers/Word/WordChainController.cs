using Signals;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers.Word
{
    public class WordChainController : MonoBehaviour
    {
        #region Serialized Variables

        [SerializeField] private GameObject wordTileRowPrefab;
        [SerializeField] private Transform chainContainer;
        [SerializeField] private ScrollRect scrollRect;

        #endregion
        
        private void OnEnable() => SubscribeEvents();
        
        private void SubscribeEvents()
        {
            WordSignals.Instance.onWordAccepted += OnWordAccepted;
            CoreGameSignals.Instance.onReset += OnReset;
        }
        
        private void OnWordAccepted(string word)
        {
            GameObject row = Instantiate(wordTileRowPrefab, chainContainer);
            WordTileController tileController = row.GetComponent<WordTileController>();
            tileController.Initialize(word);
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 0f;
        }
        
        private void OnReset()
        {
            foreach (Transform child in chainContainer)
                Destroy(child.gameObject);
        }
        
        private void UnsubscribeEvents()
        {
            WordSignals.Instance.onWordAccepted -= OnWordAccepted;
            CoreGameSignals.Instance.onReset -= OnReset;
        }
        
        private void OnDisable() => UnsubscribeEvents();
    }
}