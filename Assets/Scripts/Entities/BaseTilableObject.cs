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

        public void MoveToHero(Action CallBackMethod)
        {
            if (!_canMove)
            {
                return;
            }
            else
            {
                _path = TileController.Instance.FindPath(_currentTileBox);
                StartCoroutine(TryMoveToBox(_path[0], CallBackMethod));
            }
        }

        public void MoveFromSwipe(SwipeDirections direction, Action CallbackMethod)
        {
            switch (direction)
            {
                case SwipeDirections.Left:
                {
                    if (_currentTileBox.LeftNeighbourExists)
                    {
                        StartCoroutine(TryMoveToBox(_currentTileBox.LeftNeighbour, CallbackMethod));
                    }
                    else
                    {
                        CallbackMethod.Invoke();
                    }
                    break;
                }
                case SwipeDirections.Right:
                {
                    if (_currentTileBox.RightNeighbourExists)
                    {
                        StartCoroutine(TryMoveToBox(_currentTileBox.RightNeighbour, CallbackMethod));
                    }
                    else
                    {
                        CallbackMethod.Invoke();
                    }
                    break;
                }
                case SwipeDirections.Up:
                {
                    if (_currentTileBox.ForwardNeighbourExists)
                    {
                        StartCoroutine(TryMoveToBox(_currentTileBox.ForwardNeighbour, CallbackMethod));
                    }
                    else
                    {
                        CallbackMethod.Invoke();
                    }
                    break;
                }
                case SwipeDirections.Down:
                {
                    if (_currentTileBox.BackNeighbourExists)
                    {
                        StartCoroutine(TryMoveToBox(_currentTileBox.BackNeighbour, CallbackMethod));
                    }
                    else
                    {
                        CallbackMethod.Invoke();
                    }
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        public IEnumerator TryMoveToBox(TileBox box, Action method)
        {
            for (float i = 0; i < 1; i += 0.01f)
            {
                transform.position = Vector3.Lerp(_currentTileBox.transform.position, box.transform.position, i);
                yield return null;
            }

            SetBox(box);
            method.Invoke();
        }

        public void SetBox(TileBox box)
        {
            //_currentTileBox.changeTiledObject
            _currentTileBox = box;
            //box.changeTiledObject;
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