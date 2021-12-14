using System;
using System.Collections;
using System.Collections.Generic;
using Core.Interfaces;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Core.Entities
{
    public class TilableObject : BaseTilableObject, ITilable
    {
        [SerializeField] private bool _canMove;
        [SerializeField][Range(1,2)] private int _moveDistance;
        [SerializeField] private bool _haveSkills = false;
        [SerializeField] private List<Skill> _skills = new List<Skill>();
        
        
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
                    if (_path != null)
                    {
                        if (_path.Count > 0)
                        {
                            StartCoroutine(TryMoveToBox(_path[0], CallBackMethod, TurnState.Enemy));
                        }
                        else
                        {
                            CallBackMethod.Invoke();
                        }
                    }
                    else
                    {
                        CallBackMethod.Invoke();
                    }
                }
            }
        }

        public void MoveFromSwipe(SwipeDirections direction, Action endAnimationCallback)
        {
            if (_currentTileBox.Equals(null))
            {
                endAnimationCallback.Invoke();
                //TODO Whats going with entity without tile?
            }
            else
            {
                switch (direction)
                {
                    case SwipeDirections.Left:
                    {
                        if (_currentTileBox.LeftNeighbourExists)
                        {
                            if (_currentTileBox.LeftNeighbour.WillFree && !_currentTileBox.LeftNeighbour.TileBusy)
                                _currentTileBox.WillFree = true;
                        }
                        else
                        {
                        }

                        break;
                    }
                    case SwipeDirections.Right:
                    {
                        if (_currentTileBox.RightNeighbourExists)
                        {
                            if (_currentTileBox.RightNeighbour.WillFree && !_currentTileBox.RightNeighbour.TileBusy)
                                _currentTileBox.WillFree = true;
                        }
                        else
                        {
                            
                        }

                        break;
                    }
                    case SwipeDirections.Up:
                    {
                        if (_currentTileBox.ForwardNeighbourExists)
                        {
                            if (_currentTileBox.ForwardNeighbour.WillFree && !_currentTileBox.ForwardNeighbour.TileBusy)
                                _currentTileBox.WillFree = true;
                        }
                        else
                        {
                            
                        }

                        break;
                    }
                    case SwipeDirections.Down:
                    {
                        if (_currentTileBox.BackNeighbourExists)
                        {
                            if (_currentTileBox.BackNeighbour.WillFree && !_currentTileBox.BackNeighbour.TileBusy)
                                _currentTileBox.WillFree = true;
                        }
                        else
                        {
                            
                        }

                        break;
                    }
                    default:
                        throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
                }
                switch (direction)
                {
                    case SwipeDirections.Left:
                    {
                        if (_currentTileBox.LeftNeighbourExists)
                        {
                            StartCoroutine(TryMoveToBox(_currentTileBox.LeftNeighbour, endAnimationCallback,
                                TurnState.Player));
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
                            StartCoroutine(TryMoveToBox(_currentTileBox.RightNeighbour, endAnimationCallback,
                                TurnState.Player));
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
                            StartCoroutine(TryMoveToBox(_currentTileBox.ForwardNeighbour, endAnimationCallback,
                                TurnState.Player));
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
                            StartCoroutine(TryMoveToBox(_currentTileBox.BackNeighbour, endAnimationCallback,
                                TurnState.Player));
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

        

        public IEnumerator TryMoveToBox(TileBox box, Action endAnimationCallBack, TurnState state)
        {
            var pos = transform.position;
            if (!box.TileBusy||box.WillFree)
            {
                SetBox(box);
                for (float i = 0; i < 1; i += 0.01f * _jumpSpeed)
                {
                    TempVector3 = Vector3.Lerp(pos, box.transform.position, i);
                    TempVector3.y = Mathf.Sin(i * Mathf.PI) * _jumpHeight;
                    transform.position = TempVector3;

                    yield return null;
                }
            }
            else
            {
                switch (box.TiledObject.CompareConfig(this))
                {
                    case "Player":
                    {
                        if (state == TurnState.Enemy)
                        {
                            for (float i = 0; i < 0.5f; i += 0.01f * _jumpSpeed)
                            {
                                TempVector3 = Vector3.Lerp(_currentTileBox.transform.position, box.transform.position,
                                    i);
                                TempVector3.y = Mathf.Sin(i * Mathf.PI) * _jumpHeight;
                                transform.position = TempVector3;

                                yield return null;
                            }

                            for (float i = 0; i < 0.5f; i += 0.01f * _jumpSpeed)
                            {
                                TempVector3 = Vector3.Lerp(_currentTileBox.transform.position, box.transform.position,
                                    (0.5f - i));
                                TempVector3.y = Mathf.Sin((0.5f - i) * Mathf.PI) * _jumpHeight;
                                transform.position = TempVector3;

                                yield return null;
                            }
                        }
                        else
                        {
                            //TODO Animation blockAttack
                        }
                        break;
                    }
                    case "Enemy":
                    {
                        break;
                    }
                    default:
                        break;
                }
            }


            endAnimationCallBack.Invoke();
        }

        public void CheckBattle(string config, TurnState state)
        {
            switch (config)
            {
                case "Player":
                {
                    break;
                }
                default:
                    break;
            }
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