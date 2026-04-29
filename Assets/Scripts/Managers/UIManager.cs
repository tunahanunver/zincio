using Enums;
using Signals;
using UnityEngine;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        #region Self Variables

        #region Singleton

        public static UIManager Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance != null) { Destroy(gameObject); return; }
            Instance = this;
        }

        #endregion
        
        #region Serialized Variables
        
        [SerializeField] private Transform[] layers;
        
        #endregion
        
        #endregion
        
        private void OnEnable() => SubscribeEvents();
        
        private void SubscribeEvents()
        {
            CoreUISignals.Instance.onOpenPanel += OnOpenPanel;
            CoreUISignals.Instance.onClosePanel += OnClosePanel;
            CoreUISignals.Instance.onCloseAllPanels += OnCloseAllPanels;
            CoreGameSignals.Instance.onLevelInitialize += OnLevelInitialize;
        }
        
        private void OnLevelInitialize()
        {
            OnCloseAllPanels();
            OnOpenPanel(UIPanelType.Game);
        }
        
        private void OnOpenPanel(UIPanelType panelType)
        {
            string path = "Screens/" + panelType.ToString() + "Panel";
            GameObject prefab = Resources.Load<GameObject>(path);
            if (prefab == null)
            {
                Debug.LogError($"Panel prefab bulunamadı: {path}");
                return;
            }
            int layerIndex = GetLayerIndex(panelType);
            Instantiate(prefab, layers[layerIndex]);
        }
        
        private void OnClosePanel(UIPanelType panelType)
        {
            int layerIndex = GetLayerIndex(panelType);
            Transform layer = layers[layerIndex];
            foreach (Transform child in layer)
                Destroy(child.gameObject);
        }
        
        private void OnCloseAllPanels()
        {
            foreach (Transform layer in layers)
                foreach (Transform child in layer)
                    Destroy(child.gameObject);
        }
        
        private int GetLayerIndex(UIPanelType panelType)
        {
            switch (panelType)
            {
                case UIPanelType.Game:        return 0;
                case UIPanelType.MainMenu:    return 1;
                case UIPanelType.LevelSelect: return 1;
                case UIPanelType.Win:         return 2;
                case UIPanelType.Fail:        return 2;
                default:                      return 0;
            }
        }
        
        private void UnsubscribeEvents()
        {
            CoreUISignals.Instance.onOpenPanel -= OnOpenPanel;
            CoreUISignals.Instance.onClosePanel -= OnClosePanel;
            CoreUISignals.Instance.onCloseAllPanels -= OnCloseAllPanels;
            CoreGameSignals.Instance.onLevelInitialize -= OnLevelInitialize;
        }
        
        private void OnDisable() => UnsubscribeEvents();
    }
}