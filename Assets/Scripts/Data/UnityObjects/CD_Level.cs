using Data.ValueObjects;
using UnityEngine;

namespace Data.UnityObjects
{
    [CreateAssetMenu(fileName = "CD_Level", menuName = "zincio/CD_Level", order = 0)]
    public class CD_Level : ScriptableObject
    {
        public int levelIndex;
        public string levelName;
        public StarThresholdData starThresholds;
        public CD_WordList wordList;
    }
}