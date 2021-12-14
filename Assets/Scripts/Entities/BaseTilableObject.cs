using System;
using System.Collections;
using System.Collections.Generic;
using Core.Interfaces;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Core.Entities
{
    public class BaseTilableObject : MonoBehaviour, ITilable
    {
        [SerializeField] [Tooltip("INDEVELOPMENT")]
        private string _config;

        [SerializeField] private bool _canMove;
        [SerializeField] private bool _haveSkills = false;
        [SerializeField] private List<Skill> _skills = new List<Skill>();
        [SerializeField] private TileBox _currentTileBox;
        [SerializeField] private float _jumpHeight = .3f;
        [SerializeField] private float _jumpSpeed = 2f;
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
                if (_path == null)
                {
                    Debug.Log($"{gameObject} doesn't have path", this);
                    //TODO SKIP TURN FEEDBACK
                    CallBackMethod.Invoke();
                }
                else
                {
                    StartCoroutine(TryMoveToBox(_path[0], CallBackMethod));
                }
            }
        }

        public void MoveFromSwipe(SwipeDirections direction, Action endAnimationCallback)
        {
            if (_currentTileBox.Equals(null))
            {
                //TODO Whats going with entity without tile?`
            }
            else
            {
                switch (direction)
                {
                    case SwipeDirections.Left:
                    {
                        if (_currentTileBox.LeftNeighbourExists)
                        {
                            StartCoroutine(TryMoveToBox(_currentTileBox.LeftNeighbour, endAnimationCallback));
                        }
                        else
                        {
                            endAnimationCallback.Invoke();
                        }

                        break;
                    }
                    case SwipeDirections.Right:
                    {
                        if (_currentTileBox.RightNeighbourExists)
                        {
                            StartCoroutine(TryMoveToBox(_currentTileBox.RightNeighbour, endAnimationCallback));
                        }
                        else
                        {
                            endAnimationCallback.Invoke();
                        }

                        break;
                    }
                    case SwipeDirections.Up:
                    {
                        if (_currentTileBox.ForwardNeighbourExists)
                        {
                            StartCoroutine(TryMoveToBox(_currentTileBox.ForwardNeighbour, endAnimationCallback));
                        }
                        else
                        {
                            endAnimationCallback.Invoke();
                        }

                        break;
                    }
                    case SwipeDirections.Down:
                    {
                        if (_currentTileBox.BackNeighbourExists)
                        {
                            StartCoroutine(TryMoveToBox(_currentTileBox.BackNeighbour, endAnimationCallback));
                        }
                        else
                        {
                            endAnimationCallback.Invoke();
                        }

                        break;
                    }
                    default:
                        throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
                }
            }
        }

        private Vector3 tempVector3;

        public IEnumerator TryMoveToBox(TileBox box, Action endAnimationCallBack)
        {
            if (!box.TileBusy)
            {
                for (float i = 0; i < 1; i += 0.01f * _jumpSpeed)
                {
                    tempVector3 = Vector3.Lerp(_currentTileBox.transform.position, box.transform.position, i);
                    tempVector3.y = Mathf.Sin(i * Mathf.PI) * _jumpHeight;
                    transform.position = tempVector3;

                    yield return null;
                }
            }

            SetBox(box);
            endAnimationCallBack.Invoke();
        }

        public void SetBox(TileBox box)
        {
            if (box == null)
            {
                Debug.Log($"Enemy was destroyed becouse don't have tile");
                Destroy(this.gameObject);
                return;
            }

            if (_currentTileBox != null)
            {
                _currentTileBox.ChangeTiledObject();
            }

            _currentTileBox = box;
            box.ChangeTiledObject(this);
        }

        private bool SkillWasExecuted = false;

        public void ExecuteSkill(Action EndAnimationCallback)
        {
            SkillWasExecuted = false;
            if (_haveSkills)
            {
                for (int i = 0; i < _skills.Count; i++)
                {
                    if (!_skills[i].OnCooldown)
                    {
                        _skills[i].Execute();
                        EndAnimationCallback.Invoke();
                        SkillWasExecuted = true;
                        break;
                    }
                }
            }

            if (!SkillWasExecuted)
            {
                EndAnimationCallback.Invoke();
            }
        }
    }
}