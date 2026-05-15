using System.Collections.Generic;
using Data.ValueObjects;
using Signals;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace Controllers.Word
{
    public class WordConnectorController : MonoBehaviour
    {
        public static WordConnectorController Instance { get; private set; }
        
        [Header("References")]
        [SerializeField] private GameObject letterItemPrefab;
        [SerializeField] private RectTransform circleCenter;
        [SerializeField] private UILineRendererController lineRenderer;
        [SerializeField] private TextMeshProUGUI previewText;
        
        [Header("Settings")]
        [SerializeField] private float radius = 280f;
        private readonly List<LetterItem> _allLetters = new List<LetterItem>();
        private readonly List<LetterItem> _selectedLetters = new List<LetterItem>();
        private bool _isDragging;
        private Camera _uiCamera;
        
        private void Awake()
        {
            if (Instance != null) { Destroy(gameObject); return; }
            Instance = this;
            _uiCamera = GetComponentInParent<Canvas>().worldCamera;
        }
        
        private void OnEnable() => SubscribeEvents();
        
        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onLevelInitialize += OnLevelInitialize;
            WordSignals.Instance.onLetterSetConsumed += OnLetterSetConsumed;
            WordSignals.Instance.onWordAccepted += OnWordAccepted;
            WordSignals.Instance.onWordRejected += OnWordRejected;
            CoreGameSignals.Instance.onReset += OnReset;
        }
        
        private void Update()
        {
            if (!_isDragging) return;
            UpdateLineToPointer();
        }
        
        public void OnSelectionStarted(LetterItem item)
        {
            if (!_isDragging)
            {
                _isDragging = true;
                _selectedLetters.Clear();
            }
            AddLetter(item);
        }
        
        public void OnLetterHovered(LetterItem item)
        {
            if (!_isDragging) return;
            AddLetter(item);
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            if (!_isDragging) return;
            _isDragging = false;
            SubmitWord();
        }
        
        private void AddLetter(LetterItem item)
        {
            if (item.IsSelected) return;
            item.SetSelected(true);
            _selectedLetters.Add(item);
            UpdatePreviewText();
            UpdateLine();
        }
        
        private void SubmitWord()
        {
            if (_selectedLetters.Count == 0) return;
            string word = BuildWordFromSelection();
            WordSignals.Instance.onWordSubmitted.Invoke(word);
        }
        
        private string BuildWordFromSelection()
        {
            var sb = new System.Text.StringBuilder();
            foreach (LetterItem item in _selectedLetters)
                sb.Append(item.Letter);
            return sb.ToString();
        }
        
        private void OnLevelInitialize()
        {
            ClearLetters();
            LoadCurrentLetterSet();
        }
        private void OnLetterSetConsumed()
        {
            ClearLetters();
            LoadCurrentLetterSet();
        }
        private void LoadCurrentLetterSet()
        {
            LetterSetData set = WordSignals.Instance.onGetCurrentLetterSet.Invoke();
            if (set.letters == null || set.letters.Count == 0) return;
            ArrangeLetters(set);
        }
        
        private void ArrangeLetters(LetterSetData set)
        {
            int count = set.letters.Count;
            for (int i = 0; i < count; i++)
            {
                GameObject obj = Instantiate(letterItemPrefab, circleCenter);
                LetterItem item = obj.GetComponent<LetterItem>();
                item.Initialize(set.letters[i]);
                _allLetters.Add(item);
                // -90 derece offset: ilk harf üstte başlasın
                float angle = (360f / count) * i - 90f;
                float rad = angle * Mathf.Deg2Rad;
                Vector2 pos = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * radius;
                item.RectT.anchoredPosition = pos;
                // Giriş animasyonu
                item.transform.localScale = Vector3.zero;
                item.transform.DOScale(Vector3.one, 0.3f)
                    .SetDelay(i * 0.05f)
                    .SetEase(Ease.OutBack);
            }
        }
        
        private void ClearLetters()
        {
            foreach (LetterItem item in _allLetters)
            {
                if (item != null)
                    Destroy(item.gameObject);
            }
            _allLetters.Clear();
            _selectedLetters.Clear();
            _isDragging = false;
            lineRenderer.ClearPoints();
            if (previewText != null)
                previewText.text = string.Empty;
        }
        
        private void UpdateLine()
        {
            List<Vector2> points = new List<Vector2>();
            foreach (LetterItem item in _selectedLetters)
                points.Add(item.RectT.anchoredPosition);
            // Son nokta pointer pozisyonu (Update'de güncellenir)
            points.Add(GetPointerPositionInRect());
            lineRenderer.SetPoints(points);
        }
        
        private void UpdateLineToPointer()
        {
            if (_selectedLetters.Count == 0) return;
            List<Vector2> points = new List<Vector2>();
            foreach (LetterItem item in _selectedLetters)
                points.Add(item.RectT.anchoredPosition);
            points.Add(GetPointerPositionInRect());
            lineRenderer.SetPoints(points);
        }
        
        private Vector2 GetPointerPositionInRect()
        {
            Vector2 screenPos = Input.mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                circleCenter,
                screenPos,
                _uiCamera,
                out Vector2 localPos
            );
            return localPos;
        }
        
        private void UpdatePreviewText()
        {
            if (previewText == null) return;
            previewText.text = BuildWordFromSelection();
        }
        private void OnWordAccepted(string word)
        {
            ResetSelection();
        }
        private void OnWordRejected(string reason)
        {
            // Seçili harfleri salla (hata animasyonu)
            foreach (LetterItem item in _selectedLetters)
            {
                item.transform.DOShakePosition(0.3f, strength: 8f, vibrato: 20)
                    .OnComplete(() => item.SetSelected(false));
            }
            DOVirtual.DelayedCall(0.35f, ResetSelection);
        }
        private void ResetSelection()
        {
            foreach (LetterItem item in _selectedLetters)
                item.SetSelected(false);
            _selectedLetters.Clear();
            _isDragging = false;
            lineRenderer.ClearPoints();
            if (previewText != null)
                previewText.text = string.Empty;
        }
        private void OnReset() => ClearLetters();
        
        private void UnsubscribeEvents()
        {
            CoreGameSignals.Instance.onLevelInitialize -= OnLevelInitialize;
            WordSignals.Instance.onLetterSetConsumed -= OnLetterSetConsumed;
            WordSignals.Instance.onWordAccepted -= OnWordAccepted;
            WordSignals.Instance.onWordRejected -= OnWordRejected;
            CoreGameSignals.Instance.onReset -= OnReset;
        }
        
        private void OnDisable() => UnsubscribeEvents();
    }
}