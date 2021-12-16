using System;
using System.Collections;
using System.Collections.Generic;
using Core.Entities;
using Core.UtilitsSpace;
using UnityEngine;

namespace Core
{
    public class TilableObjectsController : MonoBehaviour
    {
        [SerializeField] private TilableObject _enemyExample;
        [SerializeField] private HealPackTilableObject _healExample;
        [SerializeField] private ExitDoorTilableObject _exitDoorExample;
        [SerializeField] private int StartEnemiesAmount;
        public List<TilableObject> _objects = new List<TilableObject>();
        private int waitingMoves = 0;
        public static TilableObjectsController Instance;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            foreach (var VARIABLE in FindObjectsOfType<TilableObject>())
            {
                if (!_objects.Contains(VARIABLE))
                {
                    VARIABLE.SetBox(TileController.Instance.GetTileForenemy());
                }
                else
                {
                    VARIABLE.SetBox(TileController.Instance.GetTileForenemy());
                }
            }
        }
        private void OnEnable()
        {
            GameEvents.Instance.OnLevelEnd += StopAllCoroutinesFroEvent;
        }
        public void OnDisable()
        {
            GameEvents.Instance.OnLevelEnd -= StopAllCoroutinesFroEvent;
        }
        public void StopAllCoroutinesFroEvent()
        {
            StopAllCoroutines();
        }

        #region ListMethods

        public void AddObjectToList(TilableObject obj)
        {
            if (!_objects.Contains(obj))
            {
                _objects.Add(obj);
            }
        }

        public void RemoveObjectFromList(TilableObject obj)
        {
            _objects.Remove(obj);
        }

        #endregion
        #region task
        TileBox _tilebox;
        TilableObject _enemy;
        public void SpawnStartEnemyes(Action forEachCall, Action onEndSpawningCallback)
        {
            for (int i = 0; i < StartEnemiesAmount; i++)
            {
                SpawnEnemy(forEachCall, onEndSpawningCallback);
            }
        }

        public void SpawnEnemy(Action forEachCall, Action onEndSpawningCallback)
        {
            forEachCall?.Invoke();
            _tilebox = TileController.Instance.GetTileForenemy();
            _enemy = Instantiate(_enemyExample.gameObject, _tilebox.transform.position + Vector3.up * 5f,
                Quaternion.identity, transform).GetComponent<TilableObject>();
            _enemy.SetBox(_tilebox);
            StartCoroutine(_enemy.SpawnAnimation((value) =>
            {
                AddObjectToList(value as TilableObject);
                onEndSpawningCallback?.Invoke();
            }));
        }
        
        public void SpawnHeal(Action forEachCall, Action onEndSpawningCallback)
        {
            forEachCall?.Invoke();
            _tilebox = TileController.Instance.GetTileForenemy();
            _enemy = Instantiate(_healExample.gameObject, _tilebox.transform.position + Vector3.up * 5f,
                Quaternion.identity, transform).GetComponent<HealPackTilableObject>();
            _enemy.SetBox(_tilebox);
            StartCoroutine(_enemy.SpawnAnimation((value) =>
            {
                AddObjectToList(value as TilableObject);
                onEndSpawningCallback?.Invoke();
            }));
        }
        public void SpawnExitDoor(Action forEachCall, Action onEndSpawningCallback) //Enter-alt
        {
            forEachCall?.Invoke();
            _tilebox = TileController.Instance.GetTileForenemy();
            _enemy = Instantiate(_exitDoorExample.gameObject, _tilebox.transform.position + Vector3.up * 5f,
                Quaternion.identity, transform).GetComponent<ExitDoorTilableObject>();
            _enemy.SetBox(_tilebox);
            StartCoroutine(_enemy.SpawnAnimation((value) =>
            {
                AddObjectToList(value as TilableObject);
                onEndSpawningCallback?.Invoke();
            }));
        }
        
        #endregion
        public IEnumerator ExecuteEnemiesSkills() //
        {
            waitingMoves = 0;
            for (int i = 0; i < _objects.Count; i++)
            {
                if (_objects[i] == null)
                {
                    continue;
                }

                if (_objects[i].HaveSkills)
                {
                    waitingMoves++;
                    _objects[i].ExecuteSkill(() => waitingMoves--);
                }

                if (_objects[i].CanMove)
                {
                    waitingMoves++;
                    _objects[i].MoveToHero(() => waitingMoves--);
                }

                yield return new WaitForSeconds(0.1f);
            }
            
            yield return new WaitUntil(() => waitingMoves == 0);
            LevelController.Instance.EndOfTurn();
        }

        public IEnumerator ExecuteEnemiesSwipeMoving(SwipeDirections direction)
        {
            waitingMoves = 0;
            for (int i = 0; i < _objects.Count; i++)
            {
                if (_objects[i] == null)
                {
                    continue;
                }

                if (_objects[i].CanMove)
                {
                    _objects[i].CheckFreeBoxState(direction);
                }
            }

            for (int i = 0; i < _objects.Count; i++)
            {
                if (_objects[i] == null)
                {
                    continue;
                }

                if (_objects[i].CanMove)
                {
                    waitingMoves++;
                    _objects[i].MoveFromSwipe(direction, () => waitingMoves--);
                }

                yield return null;
            }

            yield return new WaitUntil(() => waitingMoves == 0);
            LevelController.Instance.EndOfTurn();
        }
    }
}