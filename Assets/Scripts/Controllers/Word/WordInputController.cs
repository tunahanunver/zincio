using Signals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers.Word
{
    public class WordInputController : MonoBehaviour
    {
        #region Serialized Variables

        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private TextMeshProUGUI requiredLetterText;
        [SerializeField] private Image inputFieldBackground;
        [SerializeField] private Color defaultColor = Color.white;
        [SerializeField] private Color rejectColor = Color.red;
        [SerializeField] private Color acceptColor = Color.green;

        #endregion
        
        private void OnEnable() => SubscribeEvents();
        
        private void SubscribeEvents()
        {
            WordSignals.Instance.onWordAccepted += OnWordAccepted;
            WordSignals.Instance.onWordRejected += OnWordRejected;
            CoreGameSignals.Instance.onReset += OnReset;
        }
        
        private void OnWordAccepted(string word)
        {
            char required = WordSignals.Instance.onGetRequiredLetter.Invoke();
            requiredLetterText.text = required.ToString();
            inputFieldBackground.color = acceptColor;
            inputField.text = string.Empty;
            inputField.ActivateInputField();
            Invoke(nameof(ResetInputColor), 0.3f);
        }
        
        private void OnWordRejected(string reason)
        {
            inputFieldBackground.color = rejectColor;
            Invoke(nameof(ResetInputColor), 0.5f);
            Debug.Log("Kelime reddedildi: " + reason);
        }
        
        private void ResetInputColor()
        {
            inputFieldBackground.color = defaultColor;
        }
        
        public void OnSubmitButtonClicked()
        {
            if (string.IsNullOrWhiteSpace(inputField.text)) return;
            WordSignals.Instance.onWordSubmitted.Invoke(inputField.text);
        }
        
        private void OnReset()
        {
            inputField.text = string.Empty;
            inputFieldBackground.color = defaultColor;
            requiredLetterText.text = string.Empty;
        }
        
        private void UnsubscribeEvents()
        {
            WordSignals.Instance.onWordAccepted -= OnWordAccepted;
            WordSignals.Instance.onWordRejected -= OnWordRejected;
            CoreGameSignals.Instance.onReset -= OnReset;
        }
        
        private void OnDisable() => UnsubscribeEvents();
    }
}