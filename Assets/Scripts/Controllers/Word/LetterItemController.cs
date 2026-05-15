using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

namespace Controllers.Word
{
    public class LetterItem : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
    {
        [SerializeField] private Image background;
        [SerializeField] private TextMeshProUGUI letterText;
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color selectedColor = Color.yellow;
        public char Letter { get; private set; }
        public bool IsSelected { get; private set; }
        public RectTransform RectT { get; private set; }
        private void Awake()
        {
            RectT = GetComponent<RectTransform>();
        }
        public void Initialize(char letter)
        {
            Letter = letter;
            IsSelected = false;
            letterText.text = letter.ToString();
            background.color = normalColor;
            transform.localScale = Vector3.one;
        }
        public void SetSelected(bool selected)
        {
            IsSelected = selected;
            background.color = selected ? selectedColor : normalColor;
            transform.DOKill();
            float targetScale = selected ? 1.2f : 1f;
            transform.DOScale(targetScale, 0.15f).SetEase(Ease.OutBack);
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            WordConnectorController.Instance.OnSelectionStarted(this);
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            WordConnectorController.Instance.OnLetterHovered(this);
        }
    }
}