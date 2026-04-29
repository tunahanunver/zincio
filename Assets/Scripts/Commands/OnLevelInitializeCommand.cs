using Data.UnityObjects;

namespace Commands
{
    public class OnLevelInitializeCommand
    {
        private readonly CD_Game _cdGame;
        
        public OnLevelInitializeCommand(CD_Game cdGame)
        {
            _cdGame = cdGame;
        }
        public CD_Level Execute(int levelIndex)
        {
            int clampedIndex = levelIndex % _cdGame.levels.Count;
            return _cdGame.levels[clampedIndex];
        }
    }
}