using System;
using System.Collections;
using Core.UtilitsSpace;
using JetBrains.Annotations;
using log4net.Core;
using UnityEngine;

namespace Core
{
    public class LevelController : Singleton<LevelController>
    {
        [Header("Properties")] [SerializeField]
        private TurnState _firsTurn;

        private bool _gameEnd = false;
        private bool _endTurn = false;
        private Coroutine _turnsCoroutine;
        private TurnState _currentTurnState;

        private void Start()
        {
            _currentTurnState = _firsTurn;

            InputController.Instance.OnGetSwipe += GetSwipe;
            _turnsCoroutine = StartCoroutine(Turns()); //After All Initializations
        }

        private IEnumerator Turns()
        {
            while (!_gameEnd)
            {
                
                yield return new WaitUntil(() => _endTurn);
                yield return new WaitForSeconds(0.5f);
                Debug.Log($"Сейчас ход {_currentTurnState}");
                _endTurn = false;

                switch (_currentTurnState)
                {
                    case TurnState.Player:
                    {
                        _currentTurnState = TurnState.Enemy;
                        StartCoroutine(TilableObjectsController.Instance.ExecuteEnemiesSkills()); //Движения врагов
                        
                        break;
                    }
                    case TurnState.Enemy:
                    {
                        _currentTurnState = TurnState.Player;
                        
                        break;
                    }
                }
            }
        }

        private void GetSwipe(SwipeDirections direction)
        {
            if (_currentTurnState.Equals(TurnState.Player))
            {
                StartCoroutine(TilableObjectsController.Instance.ExecuteSwipeMoving(direction));
                /*//EndOfTurn();
                switch (direction)
                {
                    case SwipeDirections.Left:
                    {
                        TilableObjectsController.Instance.MoveEnemiesToDirection(Vector2.left);
                        break;
                    }
                    case SwipeDirections.Right:
                    {
                        TilableObjectsController.Instance.MoveEnemiesToDirection(Vector2.right);
                        break;
                    }
                    case SwipeDirections.Up:
                    {
                        TilableObjectsController.Instance.MoveEnemiesToDirection(Vector2.up);
                        break;
                    }
                    case SwipeDirections.Down:
                    {
                        TilableObjectsController.Instance.MoveEnemiesToDirection(Vector2.down);
                        break;
                    }
                    default:
                        throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
                }*/
            }
            else
            {
                
            }
        }

        #region LevelEventsCall

        public void EndOfTurn()
        {
            _endTurn = true;
        }


        public void LevelStart()
        {
            Debug.Log("Start level");
            GameEvents.Instance.LevelStart();
        }

        public void LevelVictory()
        {
            LevelEnd();
            GameEvents.Instance.LevelVictory();
        }

        public void LevelFailed()
        {
            LevelEnd();
            GameEvents.Instance.LevelFailed();
        }

        private void LevelEnd()
        {
            _gameEnd = true;
            GameEvents.Instance.LevelEnd();
        }

        #endregion
    }


    public enum TurnState
    {
        Player,
        Enemy
    }
}