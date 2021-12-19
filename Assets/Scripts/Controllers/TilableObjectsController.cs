using System;
using System.Collections;
using System.Collections.Generic;
using Core.Entities;
using Core.UtilitsSpace;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Core
{
    public class TilableObjectsController : MonoBehaviour
    {
        public static TilableObjectsController Instance;
        [SerializeField] private TilableObject _enemyExample;
        [SerializeField] private HealPackTilableObject _healExample;
        [SerializeField] private ExitDoorTilableObject _exitDoorExample;
        [SerializeField] private ProjectileTilableObject _projectile;
        [SerializeField] private TilableObject _tempBuilding;
        [SerializeField] private WeaponTilableObject[] _weapons;
        [SerializeField] private int StartEnemiesAmount;
        public List<TilableObject> _objects = new List<TilableObject>();
        private int waitingMoves = 0;
        private PlayerSkillPointer _pointer; //Enter-alt
        

        public PlayerSkillPointer Pointer => _pointer;
        
        private void Awake()
        {
            Instance = this;
            _pointer = new PlayerSkillPointer(); //Enter-alt
        }

        private void Start()
        {
            foreach (var VARIABLE in FindObjectsOfType<TilableObject>())
            {
                if (!_objects.Contains(VARIABLE))
                {
                    VARIABLE.SetBox(TileController.Instance.GetTileForenemy());
                }
                else
                {
                    VARIABLE.SetBox(TileController.Instance.GetTileForenemy());
                }
            }

            GameEvents.Instance.OnEndTouch += StopPointer; //Enter-alt
        }
        private void OnEnable()
        {
            GameEvents.Instance.OnLevelEnd += StopAllCoroutinesFroEvent;
        }
        public void OnDisable()
        {
            GameEvents.Instance.OnLevelEnd -= StopAllCoroutinesFroEvent;
        }
        public void StopAllCoroutinesFroEvent()
        {
            StopAllCoroutines();
        }

        #region ListMethods

        public void AddObjectToList(TilableObject obj)
        {
            if (!_objects.Contains(obj))
            {
                _objects.Add(obj);
            }
        }

        public void RemoveObjectFromList(TilableObject obj)
        {
            _objects.Remove(obj);
        }

        #endregion

        TileBox _tilebox;
        private Vector3 _tempPos;
        TilableObject _enemy; //ПЕРЕИМЕНУЙ (RENAME)
        #region task
        
        public void SpawnStartEnemyes(Action forEachCall, Action onEndSpawningCallback)
        {
            for (int i = 0; i < StartEnemiesAmount; i++)
            {
                SpawnEnemy(forEachCall, onEndSpawningCallback);
            }
        }

        public void SpawnEnemy(Action forEachCall, Action onEndSpawningCallback)
        {
            forEachCall?.Invoke();
            _tilebox = TileController.Instance.GetTileForenemy();
            _enemy = Instantiate(_enemyExample.gameObject, _tilebox.transform.position + Vector3.up * 5f,
                Quaternion.identity, transform).GetComponent<TilableObject>();
            _enemy.SetBox(_tilebox);
            StartCoroutine(_enemy.SpawnAnimation((value) =>
            {
                AddObjectToList(value as TilableObject);
                onEndSpawningCallback?.Invoke();
            }));
        }
        
        public void SpawnHeal(Action forEachCall, Action onEndSpawningCallback)
        {
            forEachCall?.Invoke();
            _tilebox = TileController.Instance.GetTileForenemy();
            _enemy = Instantiate(_healExample.gameObject, _tilebox.transform.position + Vector3.up * 5f,
                Quaternion.identity, transform).GetComponent<HealPackTilableObject>();
            _enemy.SetBox(_tilebox);
            StartCoroutine(_enemy.SpawnAnimation((value) =>
            {
                AddObjectToList(value as TilableObject);
                onEndSpawningCallback?.Invoke();
            }));
        }
        public void SpawnExitDoor(Action forEachCall, Action onEndSpawningCallback) //Enter-alt
        {
            forEachCall?.Invoke();
            _tilebox = TileController.Instance.GetTileForenemy();
            _enemy = Instantiate(_exitDoorExample.gameObject, _tilebox.transform.position + Vector3.up * 5f,
                Quaternion.identity, transform).GetComponent<ExitDoorTilableObject>();
            _enemy.SetBox(_tilebox);
            StartCoroutine(_enemy.SpawnAnimation((value) =>
            {
                AddObjectToList(value as TilableObject);
                onEndSpawningCallback?.Invoke();
            }));
        }
        public void SpawnWeapon(Action forEachCall, Action onEndSpawningCallback, WeaponType weaponType)
        {
            forEachCall?.Invoke();
            _tilebox = TileController.Instance.GetTileForenemy();
            _enemy = Instantiate(GetWeapon(weaponType), _tilebox.transform.position + Vector3.up * 5f,
                Quaternion.identity, transform).GetComponent<WeaponTilableObject>();
            _enemy.SetBox(_tilebox);
            StartCoroutine(_enemy.SpawnAnimation((value) =>
            {
                AddObjectToList(value as TilableObject);
                onEndSpawningCallback?.Invoke();
            }));
        }
        public void SpawnWeapon(Action forEachCall, Action onEndSpawningCallback)
        {
            SpawnWeapon(forEachCall, onEndSpawningCallback, GetRandomWeaponType());
        }
        public void SpawnProjectile(Action forEachCall, Action onEndSpawningCallback)
        {
            forEachCall?.Invoke();
            _tilebox = TileController.Instance.GetTileForenemy();
            _enemy = Instantiate(_projectile.gameObject, _tilebox.transform.position + Vector3.up * 5f,
                Quaternion.identity, transform).GetComponent<ProjectileTilableObject>();
            _enemy.SetBox(_tilebox);
            StartCoroutine(_enemy.SpawnAnimation((value) =>
            {
                AddObjectToList(value as TilableObject);
                onEndSpawningCallback?.Invoke();
            }));
        }
        
        #endregion
        #region playerPerk 
        
        #endregion //enter-alt
        public IEnumerator ExecuteEnemiesSkills() //
        {
            waitingMoves = 0;
            for (int i = 0; i < _objects.Count; i++)
            {
                if (_objects[i] == null)
                {
                    continue;
                }

                if (_objects[i].HaveSkills)
                {
                    waitingMoves++;
                    _objects[i].ExecuteSkill(() => waitingMoves--);
                }

                if (_objects[i].CanMove)
                {
                    waitingMoves++;
                    _objects[i].MoveToHero(() => waitingMoves--);
                }

                yield return new WaitForSeconds(0.1f);
            }
            
            yield return new WaitUntil(() => waitingMoves == 0);
            LevelController.Instance.EndOfTurn();
        }

        public IEnumerator ExecuteEnemiesSwipeMoving(SwipeDirections direction)
        {
            waitingMoves = 0;
            for (int i = 0; i < _objects.Count; i++)
            {
                if (_objects[i] == null)
                {
                    continue;
                }

                if (_objects[i].CanMove)
                {
                    _objects[i].CheckFreeBoxState(direction);
                }
            }

            for (int i = 0; i < _objects.Count; i++)
            {
                if (_objects[i] == null)
                {
                    continue;
                }

                if (_objects[i].CanMove)
                {
                    waitingMoves++;
                    _objects[i].MoveFromSwipe(direction, () => waitingMoves--);
                }

                yield return null;
            }

            yield return new WaitUntil(() => waitingMoves == 0);
        }

        private void StopPointer() // Enter-alt
        {
            _pointer.DropSkill();
        }

        private WeaponType GetRandomWeaponType()
        {
            return (WeaponType)Random.Range(0, _weapons.Length);
        }

        private GameObject GetWeapon(WeaponType type)
        {
            if (_weapons[(int) type])
            {
                return _weapons[(int) type].gameObject;
            }
            else
            {
                return new GameObject("NullWeapon");
            }
        }

        private void OnDestroy() // Enter-alt
        {
            GameEvents.Instance.OnEndTouch -= StopPointer; 
        }
        
    }
}