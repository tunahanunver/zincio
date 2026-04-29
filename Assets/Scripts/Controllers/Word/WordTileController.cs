using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers.Word
{
    public class WordTileController : MonoBehaviour
    {
        #region Serialized Variables

        [SerializeField] private GameObject tilePrefab;
        [SerializeField] private Transform tileContainer;
        [SerializeField] private Color defaultTileColor = Color.white;
        [SerializeField] private Color lastLetterTileColor = Color.green;

        #endregion
        
        public void Initialize(string word)
        {
            foreach (Transform child in tileContainer)
                Destroy(child.gameObject);
            for (int i = 0; i < word.Length; i++)
            {
                GameObject tile = Instantiate(tilePrefab, tileContainer);
                TextMeshProUGUI letterText = tile.GetComponentInChildren<TextMeshProUGUI>();
                Image tileImage = tile.GetComponent<Image>();
                letterText.text = word[i].ToString();
                bool isLastLetter = (i == word.Length - 1);
                tileImage.color = isLastLetter ? lastLetterTileColor : defaultTileColor;
            }
        }
    }
}