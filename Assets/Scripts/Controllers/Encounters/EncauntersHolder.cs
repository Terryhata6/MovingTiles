using System;
using System.Collections;
using Core.UtilitsSpace;
using UnityEngine;

namespace Core
{
    public class EncauntersHolder : Singleton<EncauntersHolder>
    {
        
        public IEnumerator CreateDropFromkKilledEnemy(Vector3 position)
        {
            SceneController.Instance.GetNewDraggableObject();
            yield break;
        }
        
        private IEnumerator ExecuteEncaunter(Action<Action, Action> method, Action OnStartCallBack, Action OnEndCallBack)
        {
            OnStartCallBack?.Invoke();
            int iterator = 0;
            method(() => iterator++, () => iterator--); //
            yield return new WaitUntil(() => iterator == 0);
            OnEndCallBack?.Invoke();
        }


        public void StartSpawnEnemy(Action OnStartCallBack, Action OnEndCallBack) => StartCoroutine( ExecuteStartSpawnEnemy(OnStartCallBack, OnEndCallBack));
        private IEnumerator ExecuteStartSpawnEnemy(Action OnStartCallBack, Action OnEndCallBack)
        {
            OnStartCallBack?.Invoke();
            int iterator = 0;
            TilableObjectsController.Instance.SpawnStartEnemyes(() => iterator++, () => iterator--); //
            yield return new WaitUntil(() => iterator == 0);
            OnEndCallBack?.Invoke();
        }

        public void SpawnEnemy(Action OnStartCallBack, Action OnEndCallBack) => StartCoroutine( ExecuteSpawnEnemy(OnStartCallBack, OnEndCallBack));
        private IEnumerator ExecuteSpawnEnemy(Action OnStartCallBack, Action OnEndCallBack)
        {
            OnStartCallBack?.Invoke();
            int iterator = 0;
            TilableObjectsController.Instance.SpawnEnemy(() => iterator++, () => iterator--); //
            yield return new WaitUntil(() => iterator == 0);
            OnEndCallBack?.Invoke();
        }
        
        public void SpawnHeal(Action OnStartCallBack, Action OnEndCallBack) => StartCoroutine( ExecuteSpawnHeal(OnStartCallBack, OnEndCallBack));
        private IEnumerator ExecuteSpawnHeal(Action OnStartCallBack, Action OnEndCallBack)
        {
            OnStartCallBack?.Invoke();
            int iterator = 0;
            TilableObjectsController.Instance.SpawnHeal(() => iterator++, () => iterator--); //
            yield return new WaitUntil(() => iterator == 0);
            OnEndCallBack?.Invoke();
        }
        
        public void SpawnExitDoor(Action OnStartCallBack, Action OnEndCallBack) => StartCoroutine( ExecuteExitDoor(OnStartCallBack, OnEndCallBack));
        private IEnumerator ExecuteExitDoor(Action OnStartCallBack, Action OnEndCallBack) //Enter-alt
        {
            OnStartCallBack?.Invoke();
            int iterator = 0;
            TilableObjectsController.Instance.SpawnExitDoor(() => iterator++, () => iterator--); //
            yield return new WaitUntil(() => iterator == 0);
            OnEndCallBack?.Invoke();
        }
        

        public void SpawnAxe(Action OnStartCallBack, Action OnEndCallBack) => StartCoroutine( ExecuteWeapon(OnStartCallBack, OnEndCallBack, WeaponType.Axe));
        public void SpawnPickaxe(Action OnStartCallBack, Action OnEndCallBack) => StartCoroutine( ExecuteWeapon(OnStartCallBack, OnEndCallBack, WeaponType.Pickaxe));
        public void SpawnKatana(Action OnStartCallBack, Action OnEndCallBack) => StartCoroutine( ExecuteWeapon(OnStartCallBack, OnEndCallBack, WeaponType.Katana));
        public void SpawnMace(Action OnStartCallBack, Action OnEndCallBack) => StartCoroutine( ExecuteWeapon(OnStartCallBack, OnEndCallBack, WeaponType.Mace));
        public void SpawnBigSword(Action OnStartCallBack, Action OnEndCallBack) => StartCoroutine( ExecuteWeapon(OnStartCallBack, OnEndCallBack, WeaponType.BigSword));
        private IEnumerator ExecuteWeapon(Action OnStartCallBack, Action OnEndCallBack, WeaponType weaponType) //Enter-alt
        {
            OnStartCallBack?.Invoke();
            int iterator = 0;
            TilableObjectsController.Instance.SpawnWeapon(() => iterator++, () => iterator--, weaponType); 
            yield return new WaitUntil(() => iterator == 0);
            OnEndCallBack?.Invoke();
        }
        
        public void SpawnProjectile(Action OnStartCallBack, Action OnEndCallBack) => StartCoroutine( ExecuteProjectile(OnStartCallBack, OnEndCallBack));
        private IEnumerator ExecuteProjectile(Action OnStartCallBack, Action OnEndCallBack) //Enter-alt
        {
            OnStartCallBack?.Invoke();
            int iterator = 0;
            TilableObjectsController.Instance.SpawnProjectile(() => iterator++, () => iterator--); //
            yield return new WaitUntil(() => iterator == 0);
            OnEndCallBack?.Invoke();
        }

        public void SpawnMimic(Action OnStartCallBack, Action OnEndCallBack) => StartCoroutine( ExecuteSpawnMimic(OnStartCallBack, OnEndCallBack));
        private IEnumerator ExecuteSpawnMimic(Action OnStartCallBack, Action OnEndCallBack)
        {
            OnStartCallBack?.Invoke();
            int iterator = 0;
            TilableObjectsController.Instance.SpawnMimic(() => iterator++, () => iterator--); //
            yield return new WaitUntil(() => iterator == 0);
            OnEndCallBack?.Invoke();
        }
    }
}

public enum EncaunterType
{
    StartSpawnEnemy,
    SpawnEnemy,
    SpawnHeal,
    SpawnExitDoor,
    SpawnAxe,
    SpawnPickaxe,
    SpawnKatana,
    SpawnMace,
    SpawnBigSword,
    SpawnProjectile,
    SpawnMimic
}

