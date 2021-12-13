using System.Collections.Generic;
using Core.Entities;
using Core.UtilitsSpace;
using UnityEngine;

namespace Core
{
    public class TilableObjectsController : Singleton<TilableObjectsController>
    {
        public List<BaseTilableObject> _objects = new List<BaseTilableObject>();

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

        public void ExecuteEnemiesSkills()
        {
            for (int i = 0; i < _objects.Count; i++)
            {
                if (_objects[i].HaveSkills)
                {
                    _objects[i].ExecuteSkill();
                }
            }

        }
    }
}