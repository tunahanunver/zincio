using UnityEngine;

namespace Commands
{
    public class OnLevelClearCommand
    {
        private readonly Transform _levelHolder;
        
        public OnLevelClearCommand(Transform levelHolder)
        {
            _levelHolder = levelHolder;
        }
        public void Execute()
        {
            if (_levelHolder.childCount == 0) return;
            Object.Destroy(_levelHolder.GetChild(0).gameObject);
        }
    }
}