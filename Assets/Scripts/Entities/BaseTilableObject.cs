using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Core.Entities
{
    public class BaseTilableObject : MonoBehaviour
    {
        [SerializeField] [Tooltip("INDEVELOPMENT")]
        private string _config;

        [SerializeField] private bool _canMove;
        [SerializeField] private bool _haveSkills = false;
        [SerializeField] private List<Skill> _skills = new List<Skill>();
        [SerializeField] private TileBox _currentTileBox;
        public bool HaveSkills => _haveSkills;
        public bool CanMove => _canMove;

        public void Awake()
        {
            if (_skills.Count > 0)
            {
                _haveSkills = true;
            }
        }


        private List<TileBox> _path;

        public void Move(Action CallBackMethod)
        {
            if (!_canMove)
            {
                return;
            }
            else
            {
                _path = TileController.Instance.FindPath(_currentTileBox);
                /*for (int i = 0; i < _path.Count - 1; i++)
                {
                    Debug.Log("Going to path");
                    if (_path[i].Equals(_currentTileBox))
                    {
                        StartCoroutine(TryMoveToBox(_path[i+1], CallBackMethod));
                    }
                    else
                    {
                        Debug.Log("notThis");
                    }
                    
                }*/
                StartCoroutine(TryMoveToBox(_path[0], CallBackMethod));
            }
            
        }

        public IEnumerator TryMoveToBox(TileBox box, Action method)
        {
            for (float i = 0; i < 1; i+=0.01f)
            {
                transform.position = Vector3.Lerp(_currentTileBox.transform.position, box.transform.position, i);
                yield return null;
            }

            _currentTileBox = box;
            method.Invoke();
        }

        public void SetBox(TileBox box)
        {
            //MovetoBoxinstantly
        }

        private bool SkillWasExecuted = false;
        public void ExecuteSkill(Action CallbackMethod)
        {
            SkillWasExecuted = false;
            if (_haveSkills)
            {
                for (int i = 0; i < _skills.Count; i++)
                {
                    if (!_skills[i].OnCooldown)
                    {
                        _skills[i].Execute();
                        CallbackMethod.Invoke();
                        SkillWasExecuted = true;
                        break;
                    }
                }
            }

            if (!SkillWasExecuted)
            {
                CallbackMethod.Invoke();
            }
        }
    }
}