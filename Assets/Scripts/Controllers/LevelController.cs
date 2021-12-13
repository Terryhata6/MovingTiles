using UnityEngine;

namespace Core
{
    public class LevelController : MonoBehaviour
    {





        #region LevelEventsCall
        public void LevelStart()
        {
            Debug.Log("Start level");
            GameEvents.Instance.LevelStart();
        }

        public void LevelVictory()
        {
            LevelEnd();
            GameEvents.Instance.LevelVictory();
        }

        public void LevelFailed()
        {
            LevelEnd();
            GameEvents.Instance.LevelFailed();
        }

        private void LevelEnd()
        {
            GameEvents.Instance.LevelEnd();
        }
        #endregion
    }
}