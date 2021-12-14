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
        [SerializeField] private BaseTilableObject _enemyExample;
        public List<BaseTilableObject> _objects = new List<BaseTilableObject>();
        private int waitingMoves = 0;

        private void Start()
        {
            foreach (var VARIABLE in  FindObjectsOfType<BaseTilableObject>())
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
        public void AddObjectToList(BaseTilableObject obj)
        {
            if (!_objects.Contains(obj))
            {
                _objects.Add(obj);
            }
        }

        public void RemoveObjectFromList(BaseTilableObject obj)
        {
            _objects.Remove(obj);
        }
        #endregion

        public void SpawnStartEnemyes()
        {
            
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