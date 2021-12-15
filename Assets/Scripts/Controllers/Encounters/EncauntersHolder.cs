using System;
using System.Collections;
using Core.UtilitsSpace;
using UnityEngine;

namespace Core
{
    public class EncauntersHolder : Singleton<EncauntersHolder>
    {
        
        public void StartSpawnEnemy(Action OnStartCallBack, Action OnEndCallBack) => StartCoroutine( ExecuteStartSpawnEnemy(OnStartCallBack, OnEndCallBack));
        private IEnumerator ExecuteStartSpawnEnemy(Action OnStartCallBack, Action OnEndCallBack)
        {
            OnStartCallBack?.Invoke();
            int iterator = 0;
            TilableObjectsController.Instance.SpawnStartEnemyes(() => iterator++, () => iterator--);
            yield return new WaitUntil(() => iterator == 0);
            OnEndCallBack?.Invoke();
        }
    }
}