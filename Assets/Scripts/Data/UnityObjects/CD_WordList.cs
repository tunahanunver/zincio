using System.Collections.Generic;
using UnityEngine;

namespace Data.UnityObjects
{
    [CreateAssetMenu(fileName = "CD_WordList", menuName = "zincio/CD_WordList", order = 0)]
    public class CD_WordList : ScriptableObject
    {
        public List<string> words = new List<string>();
    }
}