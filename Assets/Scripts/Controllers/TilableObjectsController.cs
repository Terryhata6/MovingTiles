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
        public List<BaseTilableObject> _objects = new List<BaseTilableObject>();
        private int waitingMoves = 0;

        private void Start()
        {
            
        }
        
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


        public void MoveEnemiesToDirection(Vector2 direction)
        {
            
        }

        public IEnumerator ExecuteEnemiesSkills()
        {
            waitingMoves = 0;
            for (int i = 0; i < _objects.Count; i++)
            {
                if (_objects[i].HaveSkills)
                {
                    waitingMoves++;
                    _objects[i].ExecuteSkill(() => waitingMoves--);
                }

                if (_objects[i].CanMove)
                {
                    waitingMoves++;
                    _objects[i].Move( () => waitingMoves--);
                }
            }
            yield return new WaitUntil(() => waitingMoves == 0);
            LevelController.Instance.EndOfTurn();
        }

        
    }
}