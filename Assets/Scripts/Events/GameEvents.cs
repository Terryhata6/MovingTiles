using System;
using Core.UtilitsSpace;

namespace Core
{
    public class GameEvents : Singleton<GameEvents>
    {
        public event Action OnLevelVictory;
        public void LevelVictory()
        {
            OnLevelVictory?.Invoke();
        }

        public event Action OnLevelFailed;
        public void LevelFailed()
        {
            OnLevelFailed?.Invoke();
        }

        public event Action OnLevelStart;
        public void LevelStart()
        {
            OnLevelStart?.Invoke();
        }

        public event Action OnLevelEnd;
        public void LevelEnd()
        {
            OnLevelEnd?.Invoke();
        }
    }
}