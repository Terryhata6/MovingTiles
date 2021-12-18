using System;
using System.Collections;
using System.Collections.Generic;
using Core.UtilitsSpace;
using UnityEngine;

namespace Core
{
    public class LevelController : MonoBehaviour
    {
        public static LevelController Instance;
        [Header("Properties")] [SerializeField]
        private TurnState _firstTurn;

        [SerializeField]private bool _gameEnd = false;
        [SerializeField]private bool _endTurn = false;
        private Coroutine _turnsCoroutine;
        [SerializeField]private TurnState _currentTurnState;
        [SerializeField]private int _turnNumber = 0;
        [SerializeField] private int _currentTasksNum = 0;
        private Dictionary<int, Action<Action,Action>> turnTasks = new Dictionary<int, Action<Action,Action>>();

        public TurnState TurnState => _currentTurnState;
        public int TurnNumber => _turnNumber;

        private void Awake()
        {
            Instance = this;
            GameEvents.Instance.LoadNewLevelController(this);
        }

        private void Start()
        { 
            
        }

        public void Initialize()
        {
            TileController.Instance.CreateTiles();
            CreateNewTask(0, EncauntersHolder.Instance.StartSpawnEnemy);
            CreateNewTask(4, EncauntersHolder.Instance.SpawnEnemy);
            CreateNewTask(4, EncauntersHolder.Instance.SpawnHeal);
            CreateNewTask(4, EncauntersHolder.Instance.SpawnExitDoor);
            CreateNewTask(6, EncauntersHolder.Instance.SpawnEnemy);
            //TilableObjectsController.Instance.SpawnStartEnemyes
            
            _currentTurnState = (_firstTurn == TurnState.Player)?TurnState.Enemy:TurnState.Player;


            _endTurn=(_currentTurnState != TurnState.Enemy);
        }

        private IEnumerator Turns()
        {
            _turnNumber = 0;
            if (_gameEnd)
            {
                yield break;
            }

            while (!_gameEnd)
            {
                Debug.Log($"Начало Цикла, end of turn {_endTurn}");
                yield return new WaitForSeconds(0.5f);
                yield return new WaitUntil(() => _endTurn);
                _endTurn = false;

                switch (_currentTurnState)
                {
                    case TurnState.Player:
                    {
                        _currentTurnState = TurnState.Enemy;   //start enemy turn
                        Debug.Log($"Сейчас ход {_currentTurnState}");
                        StartCoroutine(TilableObjectsController.Instance.ExecuteEnemiesSkills()); //Движения врагов
                        
                        if (turnTasks.ContainsKey(TurnNumber))
                        {
                            _currentTasksNum = 0;
                            Debug.Log($"Release tasks from turn {TurnNumber}");
                            turnTasks[TurnNumber]?.Invoke(() => _currentTasksNum++, () => _currentTasksNum--);
                            yield return new WaitUntil(() =>  _currentTasksNum == 0);
                            Debug.Log($"tasks from turn {TurnNumber} done");
                            turnTasks.Remove(TurnNumber);
                        }
                        break;
                    }
                    case TurnState.Enemy: 
                    {
                        _currentTurnState = TurnState.Player; //start player turn
                        InputController.Instance.OnGetSwipe += GetSwipe;
                        Debug.Log($"Сейчас ход {_currentTurnState}");
                        _turnNumber++;
                        
                        break;
                    }
                }
                Debug.Log($"Конец Цикла");
            }
            //todo endGame animations
        }

        private void CreateNewTask(int turnNumber, Action<Action,Action> method)
        {
            if (!turnTasks.ContainsKey(turnNumber))
            {
                turnTasks.Add(turnNumber, delegate {  });
            }
            turnTasks[turnNumber] += method;
        }

        private void GetSwipe(SwipeDirections direction)
        {
            StartCoroutine(GetSwipeCoroutine(direction));
        }

        private IEnumerator GetSwipeCoroutine(SwipeDirections direction)
        {
            if (_currentTurnState.Equals(TurnState.Player))
            {
                InputController.Instance.OnGetSwipe -= GetSwipe;
                yield return TilableObjectsController.Instance.ExecuteEnemiesSwipeMoving(direction);
                EndOfTurn();
            }
        }

        #region LevelEventsCall

        public void EndOfTurn()
        {
            Debug.Log($"Call End of turn");
            _endTurn = true;
        }


        public void LevelStart()
        {
            Debug.Log("Start level");
            GameEvents.Instance.LevelStart();
            
            _turnsCoroutine = StartCoroutine(Turns()); //After All Initializations
        }

        public void LevelVictory()
        {
            LevelEnd();
            GameEvents.Instance.LevelVictory();
        }

        public IEnumerator LevelFailed()
        {
            LevelEnd();
            yield return new WaitForSeconds(1f);
            GameEvents.Instance.LevelFailed();
            
        }

        private void LevelEnd()
        {
            _gameEnd = true;
            StopCoroutine(_turnsCoroutine);
            StopAllCoroutines();
            InputController.Instance.OnGetSwipe -= GetSwipe;
            GameEvents.Instance.LevelEnd();
            
        }

        #endregion

        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }


    public enum TurnState
    {
        Player,
        Enemy
    }
}