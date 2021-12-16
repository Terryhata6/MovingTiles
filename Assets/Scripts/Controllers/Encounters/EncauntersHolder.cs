using System;
using System.Collections;
using Core.UtilitsSpace;
using UnityEngine;

namespace Core
{
    public class EncauntersHolder : Singleton<EncauntersHolder>
    {
        private IEnumerator ExecuteEncaunter(Action<Action, Action> method, Action OnStartCallBack, Action OnEndCallBack)
        {
            OnStartCallBack?.Invoke();
            int iterator = 0;
            method(() => iterator++, () => iterator--); ///
            yield return new WaitUntil(() => iterator == 0);
            OnEndCallBack?.Invoke();
        }


        public void StartSpawnEnemy(Action OnStartCallBack, Action OnEndCallBack) => StartCoroutine( ExecuteStartSpawnEnemy(OnStartCallBack, OnEndCallBack));
        private IEnumerator ExecuteStartSpawnEnemy(Action OnStartCallBack, Action OnEndCallBack)
        {
            OnStartCallBack?.Invoke();
            int iterator = 0;
            TilableObjectsController.Instance.SpawnStartEnemyes(() => iterator++, () => iterator--); ///
            yield return new WaitUntil(() => iterator == 0);
            OnEndCallBack?.Invoke();
        }

        public void SpawnEnemy(Action OnStartCallBack, Action OnEndCallBack) => StartCoroutine( ExecuteSpawnEnemy(OnStartCallBack, OnEndCallBack));
        private IEnumerator ExecuteSpawnEnemy(Action OnStartCallBack, Action OnEndCallBack)
        {
            OnStartCallBack?.Invoke();
            int iterator = 0;
            TilableObjectsController.Instance.SpawnEnemy(() => iterator++, () => iterator--); ///
            yield return new WaitUntil(() => iterator == 0);
            OnEndCallBack?.Invoke();
        }
        
        public void SpawnHeal(Action OnStartCallBack, Action OnEndCallBack) => StartCoroutine( ExecuteSpawnHeal(OnStartCallBack, OnEndCallBack));
        private IEnumerator ExecuteSpawnHeal(Action OnStartCallBack, Action OnEndCallBack)
        {
            OnStartCallBack?.Invoke();
            int iterator = 0;
            TilableObjectsController.Instance.SpawnHeal(() => iterator++, () => iterator--); ///
            yield return new WaitUntil(() => iterator == 0);
            OnEndCallBack?.Invoke();
        }
        
        public void SpawnExitDoor(Action OnStartCallBack, Action OnEndCallBack) => StartCoroutine( ExecuteExitDoor(OnStartCallBack, OnEndCallBack));
        private IEnumerator ExecuteExitDoor(Action OnStartCallBack, Action OnEndCallBack) //Enter-alt
        {
            OnStartCallBack?.Invoke();
            int iterator = 0;
            TilableObjectsController.Instance.SpawnExitDoor(() => iterator++, () => iterator--); ///
            yield return new WaitUntil(() => iterator == 0);
            OnEndCallBack?.Invoke();
        }
        
        public void SpawnBuilding(Action OnStartCallBack, Action OnEndCallBack) => StartCoroutine( ExecuteBuilding(OnStartCallBack, OnEndCallBack));
        private IEnumerator ExecuteBuilding(Action OnStartCallBack, Action OnEndCallBack) //Enter-alt
        {
            OnStartCallBack?.Invoke();
            int iterator = 0;
            TilableObjectsController.Instance.SpawnBuilding(() => iterator++, () => iterator--); ///
            yield return new WaitUntil(() => iterator == 0);
            OnEndCallBack?.Invoke();
        }
    }
}