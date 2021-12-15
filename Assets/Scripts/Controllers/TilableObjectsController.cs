using System;
using System.Collections;
using System.Collections.Generic;
using Core.Entities;
using Core.UtilitsSpace;
using UnityEngine;

namespace Core
{
    public class TilableObjectsController : Singleton<TilableObjectsController>
    {
        [SerializeField] private TilableObject _enemyExample;
        [SerializeField] private int StartEnemiesAmount;
        public List<TilableObject> _objects = new List<TilableObject>();
        private int waitingMoves = 0;

        private void Start()
        {
            foreach (var VARIABLE in  FindObjectsOfType<TilableObject>())
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

        public void SpawnStartEnemyes(Action forEachCall, Action onEndSpawningCallback)
        {
            TileBox tilebox;
            TilableObject enemy;
            for (int i = 0; i < StartEnemiesAmount; i++)
            {
                forEachCall?.Invoke();
                tilebox = TileController.Instance.GetTileForenemy();
                enemy = Instantiate(_enemyExample.gameObject, tilebox.transform.position + Vector3.up * 5f, Quaternion.identity).GetComponent<TilableObject>();
                enemy.SetBox(tilebox);
                StartCoroutine(enemy.SpawnAnimation((value) =>
                {
                    AddObjectToList(value as TilableObject);
                    onEndSpawningCallback?.Invoke();
                }));
            }
        }

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
                    _objects[i].MoveToHero( () => waitingMoves--);
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
                    _objects[i].MoveFromSwipe( direction,() => waitingMoves--);
                }
                yield return null;
            }
            yield return new WaitUntil(() => waitingMoves == 0);
            LevelController.Instance.EndOfTurn();
        }
    }
}