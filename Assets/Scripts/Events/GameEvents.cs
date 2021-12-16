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

        public event Action OnRestartLevel;
        public void RestartLevel()
        {
            OnRestartLevel?.Invoke();
        }

        public event Action<float,float,float> OnPlayerHpChange;
        public void PlayerHpChange(float currentValue,float minValue, float maximumValue)
        {
            OnPlayerHpChange?.Invoke(currentValue, minValue, maximumValue);
        }
    }
}