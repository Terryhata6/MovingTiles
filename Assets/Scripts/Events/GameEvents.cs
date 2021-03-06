using System;
using Core.UtilitsSpace;
using UnityEngine;

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

        public event Action OnEndTouch; //eNTER-ALT

        public void EndTouch() //eNTER-ALT
        {
            OnEndTouch?.Invoke();
        }

        public event Action<float,float,float> OnPlayerHpChange;
        public void PlayerHpChange(float currentValue,float minValue, float maximumValue)
        {
            OnPlayerHpChange?.Invoke(currentValue, minValue, maximumValue);
        }

        public event Action<LevelController> OnLoadNewLevelController;
        public void LoadNewLevelController(LevelController controller)
        {
            OnLoadNewLevelController?.Invoke(controller);
        }

        public event Action OnGetDraggableDrop;

        public void GetDraggableDrop()
        {
            OnGetDraggableDrop?.Invoke();
        }

        public Action OnHideLevelUI;
        public void HideLevelUI()
        {
            OnHideLevelUI?.Invoke();
        }

        public Action<Vector3> OnSwipeFromArrow;
        public void SwipeFromArrow(Vector3 dir)
        {
            OnSwipeFromArrow?.Invoke(dir);
        }
    }
}