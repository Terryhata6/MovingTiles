using System;
using System.Collections;
using System.Collections.Generic;
using Core.UtilitsSpace;
using UnityEngine;
using Cinemachine;


namespace Core
{
    public class LevelController : MonoBehaviour
    {
        public static LevelController Instance;
        [Header("Properties")] [SerializeField]
        private TurnState _firstTurn;
        [SerializeField]private EnviSetType setTypeType;
        [SerializeField]private bool _gameEnd = false;
        [SerializeField]private bool _endTurn = false;
        private Coroutine _turnsCoroutine;
        [Header("Tasks")][SerializeField]private TurnState _currentTurnState;
        [SerializeField]private int _turnNumber = 0;
        [SerializeField] private int _currentTasksNum = 0;
        private Dictionary<int, Action<Action,Action>> turnTasks = new Dictionary<int, Action<Action,Action>>();

        [Header("Scenarios")] [SerializeField] private List<LevelScenario> _scenarios;
        [Header("Camera")]
        [SerializeField] private CinemachineVirtualCamera _mobileCamera;
        [SerializeField] private CinemachineVirtualCamera _pcCamera;



        public TurnState TurnState => _currentTurnState;
        public int TurnNumber => _turnNumber;

        private void Awake()
        {
            Instance = this;
            GameEvents.Instance.LoadNewLevelController(this);
        }

        public void Initialize()
        {
            TileController.Instance.CreateTiles();
            
            //TilableObjectsController.Instance.SpawnStartEnemyes
            foreach (var var in _scenarios[PlayerPrefs.GetInt("CurrentZone")].tasks)
            {
                CreateNewTask(var.turn, var.taskType);
            }
            EnviermentController.Instance.ActivateSet(_scenarios[PlayerPrefs.GetInt("CurrentZone")].envi);
            
            _currentTurnState = (_firstTurn == TurnState.Player)?TurnState.Enemy:TurnState.Player;


            _endTurn=(_currentTurnState != TurnState.Enemy);
            

            if (Screen.width > Screen.height)
            {
                _pcCamera.Priority = 99;
                _mobileCamera.Priority = 10;
            }
            else
            {
                _mobileCamera.Priority = 99;
                _pcCamera.Priority = 10;
            }
        }

        private Action<Action, Action> taskMethod;
        public void CreateNewTask(int turn, EncaunterType type)
        {
            switch (type)
            {
                case EncaunterType.StartSpawnEnemy:
                    taskMethod = EncauntersHolder.Instance.StartSpawnEnemy;
                    break;
                case EncaunterType.SpawnEnemy:
                    taskMethod = EncauntersHolder.Instance.SpawnEnemy;
                    break;
                case EncaunterType.SpawnHeal:
                    taskMethod = EncauntersHolder.Instance.SpawnHeal;
                    break;
                case EncaunterType.SpawnExitDoor:
                    taskMethod = EncauntersHolder.Instance.SpawnExitDoor;
                    break;
                case EncaunterType.SpawnAxe:
                    taskMethod = EncauntersHolder.Instance.SpawnAxe;
                    break;
                case EncaunterType.SpawnPickaxe:
                    taskMethod = EncauntersHolder.Instance.SpawnPickaxe;
                    break;
                case EncaunterType.SpawnKatana:
                    taskMethod = EncauntersHolder.Instance.SpawnKatana;
                    break;
                case EncaunterType.SpawnMace:
                    taskMethod = EncauntersHolder.Instance.SpawnMace;
                    break;
                case EncaunterType.SpawnBigSword:
                    taskMethod = EncauntersHolder.Instance.SpawnBigSword;
                    break;
                case EncaunterType.SpawnProjectile:
                    taskMethod = EncauntersHolder.Instance.SpawnProjectile;
                    break;
                case EncaunterType.SpawnMimic:
                    taskMethod = EncauntersHolder.Instance.SpawnMimic;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            CreateNewTask(turn, taskMethod);
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
                //Debug.Log($"Начало Цикла, end of turn {_endTurn}");
                yield return new WaitForSeconds(0.5f);
                yield return new WaitUntil(() => _endTurn);
                _endTurn = false;

                switch (_currentTurnState)
                {
                    case TurnState.Player:
                    {
                        _currentTurnState = TurnState.Enemy;   //start enemy turn
                        //Debug.Log($"Сейчас ход {_currentTurnState}");
                        StartCoroutine(TilableObjectsController.Instance.ExecuteEnemiesSkills()); //Движения врагов
                        
                        if (turnTasks.ContainsKey(TurnNumber))
                        {
                            _currentTasksNum = 0;
                            //Debug.Log($"Release tasks from turn {TurnNumber}");
                            turnTasks[TurnNumber]?.Invoke(() => _currentTasksNum++, () => _currentTasksNum--);
                            yield return new WaitUntil(() =>  _currentTasksNum == 0);
                            //Debug.Log($"tasks from turn {TurnNumber} done");
                            turnTasks.Remove(TurnNumber);
                        }
                        break;
                    }
                    case TurnState.Enemy: 
                    {
                        _currentTurnState = TurnState.Player; //start player turn
                        InputController.Instance.OnGetSwipe += GetSwipe;
                        //Debug.Log($"Сейчас ход {_currentTurnState}");
                        _turnNumber++;
                        
                        break;
                    }
                }
                //Debug.Log($"Конец Цикла");
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
            //Debug.Log($"Call End of turn");
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
            SaveLoadManager.Instance.OnDeath();
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

    [Serializable]
    public struct LevelScenario
    {
        public List<LevelTask> tasks;
        public EnviSetType envi;
    }
    [Serializable]
    public struct LevelTask
    {
        public int turn;
        public EncaunterType taskType;
    }
}