using System.Collections.Generic;
using Data.ValueObjects;
using UnityEngine;

namespace Data.UnityObjects
{
    [CreateAssetMenu(fileName = "CD_Game", menuName = "zincio/CD_Game", order = 0)]
    public class CD_Game : ScriptableObject
    {
        public float gameDuration = 60f;
        public StarThresholdData defaultStarThresholds;
        public List<CD_Level> levels = new List<CD_Level>();
    }
}