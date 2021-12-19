using System;
using System.Collections;
using System.Collections.Generic;
using Core.Interfaces;
using DG.Tweening;
using UnityEngine;

namespace Core.Entities
{
    public class TilableObject : BaseTilableObject, ITilable
    {
        [SerializeField] private bool _canMove;
        [SerializeField][Range(1,2)] private int _moveDistance;
        [SerializeField] private bool _haveSkills = false;
        [SerializeField] private List<Skill> _skills = new List<Skill>();
        [SerializeField] private bool _needDebugLog = false;
        [SerializeField] private bool _wantToMoveToHero = false;
        
        
        public bool HaveSkills => _haveSkills;
        public bool CanMove => _canMove;

        public override void Awake()
        {
            base.Awake();
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
            else if(_wantToMoveToHero)
            {
                _path = TileController.Instance.FindPath(_currentTileBox);
                if (_path == null)
                {

                    if(_needDebugLog)
                    {
                        Debug.Log($"{gameObject} doesn't have path", this);
                    }        
                    CallBackMethod.Invoke();
                    if(_needDebugLog)
                    {
                        Debug.Log($"{gameObject} still invoked", this);
                    } 
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
            else
            {
                CallBackMethod.Invoke();
            }
        }


        public void CheckFreeBoxState(SwipeDirections direction)
        {
            switch (direction)
            {
                case SwipeDirections.Left:
                {
                    if (_currentTileBox.LeftNeighbourExists)
                    {
                        if (_currentTileBox.LeftNeighbour.WillFree || !_currentTileBox.LeftNeighbour.TileBusy)
                        {
                            _currentTileBox.WillFree = true;
                        }
                    }
                    break;
                }
                case SwipeDirections.Right:
                {
                    if (_currentTileBox.RightNeighbourExists)
                    {
                        if (_currentTileBox.RightNeighbour.WillFree || !_currentTileBox.RightNeighbour.TileBusy)
                            _currentTileBox.WillFree = true;
                    }
                    break;
                }
                case SwipeDirections.Up:
                {
                    if (_currentTileBox.ForwardNeighbourExists)
                    {
                        if (_currentTileBox.ForwardNeighbour.WillFree || !_currentTileBox.ForwardNeighbour.TileBusy)
                            _currentTileBox.WillFree = true;
                    }
                    break;
                }
                case SwipeDirections.Down:
                {
                    if (_currentTileBox.BackNeighbourExists)
                    {
                        
                        if (_currentTileBox.BackNeighbour.WillFree || !_currentTileBox.BackNeighbour.TileBusy)
                            _currentTileBox.WillFree = true;
                    }
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
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
            if (this.gameObject == null)
            {
                endAnimationCallBack.Invoke();
                yield break;
            }

            transform.DOLookAt(box.transform.position, 0.05f);
            var pos = transform.position;
            
            if (!box.TileBusy || box.WillFree)
            {
                SetBox(box);
                for (float i = 0; i < 1; i += Time.deltaTime * _jumpSpeed)
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
                        yield return StartCoroutine(InteractionWithPlayer(box, state));
                        break;
                    }
                    case "Enemy":
                    {
                        break;
                    }
                    case "Building":
                    {
                        break;
                    }
                    case "Weapon":
                    {
                        break;
                    }
                    case "Collectable":
                    {
                        break;
                    }
                    default:
                        break;
                }
            }
            endAnimationCallBack.Invoke();
        }

        protected override IEnumerator InteractionWithPlayer(TileBox box, TurnState state)
        {
            //base.InteractionWithPlayer(box, state);
            yield break;
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