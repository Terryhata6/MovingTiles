using System;
using System.Collections;
using System.Collections.Generic;
using Core.UtilitsSpace;
using JetBrains.Annotations;
using log4net.Core;
using UnityEngine;

namespace Core
{
    public class LevelController : Singleton<LevelController>
    {
        [Header("Properties")] [SerializeField]
        private TurnState _firstTurn;

        private bool _gameEnd = false;
        private bool _endTurn = false;
        private Coroutine _turnsCoroutine;
        private TurnState _currentTurnState;
        private int _turnNumber = 0;
        private Dictionary<int, Action> turnTasks = new Dictionary<int, Action>();
        
        
        
        public TurnState TurnState => _currentTurnState;
        public int TurnNumber => _turnNumber;
        
        private void Start()
        {
            TileController.Instance.CreateTiles();
            CreateNewTask(0, TilableObjectsController.Instance.SpawnStartEnemyes);
            
            
            _currentTurnState = _firstTurn;

            
            
        }

        private IEnumerator Turns()
        {
            _turnNumber = 0;
            while (!_gameEnd)
            {
                yield return new WaitUntil(() => _endTurn);
                yield return new WaitForSeconds(0.5f);
                _endTurn = false;

                switch (_currentTurnState)
                {
                    case TurnState.Player:
                    {
                        _currentTurnState = TurnState.Enemy;
                        Debug.Log($"Сейчас ход {_currentTurnState}");
                        StartCoroutine(TilableObjectsController.Instance.ExecuteEnemiesSkills()); //Движения врагов
                        
                        if (turnTasks.ContainsKey(TurnNumber))
                        {
                            turnTasks[TurnNumber]?.Invoke();
                            turnTasks.Remove(TurnNumber);
                        }
                        break;
                    }
                    case TurnState.Enemy:
                    {
                        _currentTurnState = TurnState.Player;
                        Debug.Log($"Сейчас ход {_currentTurnState}");
                        _turnNumber++;
                        
                        break;
                    }
                }
            }
        }

        private void CreateNewTask(int turnNumber, Action method)
        {
            if (!turnTasks.ContainsKey(turnNumber))
            {
                turnTasks.Add(turnNumber, delegate { Debug.Log($"Release tasks from turn {turnNumber}"); });
            }
            turnTasks[turnNumber] += method;
        }

        private void GetSwipe(SwipeDirections direction)
        {
            if (_currentTurnState.Equals(TurnState.Player))
            {
                StartCoroutine(TilableObjectsController.Instance.ExecuteEnemiesSwipeMoving(direction));
                
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
            InputController.Instance.OnGetSwipe += GetSwipe;
            _turnsCoroutine = StartCoroutine(Turns()); //After All Initializations
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
            InputController.Instance.OnGetSwipe -= GetSwipe;
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